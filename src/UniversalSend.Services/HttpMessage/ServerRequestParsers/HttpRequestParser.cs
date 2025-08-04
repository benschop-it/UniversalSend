using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models.Common;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Controllers;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Plumbing;
using UniversalSend.Services.Interfaces;
using UniversalSend.Services.Interfaces.Internal;
using Windows.Storage.Streams;

namespace UniversalSend.Services.HttpMessage.ServerRequestParsers {

    internal class HttpRequestParser : IHttpRequestParser {

        #region Private Fields

        private const uint BUFFER_SIZE = 8192;
        private readonly ILogger _logger;

        // ---- Adaptive cadence knobs (private; tweak as desired) ----

        // <= 256 KiB
        private readonly long _smallFileBytesThreshold = 256 * 1024;

        // <= 2 MiB
        private readonly long _mediumFileBytesThreshold = 2 * 1024 * 1024;

        // single in-progress tick (<=99%), then Finished(100)
        private readonly int _smallFileSteps = 1;

        // ~25% steps
        private readonly int _mediumFileSteps = 4;

        // ~10% steps
        private readonly int _largeFileSteps = 10;

        // notify each MiB
        private readonly long _unknownLengthChunkBytes = 1 * 1024 * 1024;

        #endregion Private Fields

        #region Public Constructors

        public HttpRequestParser() {
            _logger = LogManager.GetLogger<HttpRequestParser>();
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler<HttpParseProgressEventArgs> ProgressChanged;

        #endregion Public Events

        #region Public Methods

        public async Task<IMutableHttpServerRequest> ParseRequestStream(IInputStream requestStream) {
            var httpStream = new HttpRequestStream(requestStream);
            var request = new MutableHttpServerRequest(this);

            long contentLength = -1;
            long received = 0;

            int totalSteps = 0;
            int stepsEmitted = 0;
            long stepBytes = 0;
            long nextKnownThreshold = long.MaxValue;
            long nextUnknownThreshold = _unknownLengthChunkBytes;

            try {
                var first = await httpStream.ReadAsync(BUFFER_SIZE, InputStreamOptions.Partial);
                byte[] streamData = first.Data;
                received += streamData.Length;

                var requestPipeline = GetPipeline();
                using (var it = requestPipeline.GetEnumerator()) {
                    if (!it.MoveNext()) return request;   // defensive

                    bool done = false;

                    while (!done) {
                        it.Current.HandleRequestPart(streamData, request);
                        streamData = it.Current.UnparsedData;

                        if (it.Current.IsFinished) {
                            if (!it.Current.IsSucceeded || !it.MoveNext()) {
                                break;
                            }
                        } else {
                            // Capture content length once (after headers are parsed)
                            if (contentLength < 0 && request.ContentLength > 0) {
                                contentLength = request.ContentLength;
                                totalSteps = StepCountFor(contentLength);

                                if (totalSteps > 0) {
                                    // Compute step size and first threshold
                                    stepBytes = contentLength / totalSteps;
                                    if (stepBytes <= 0) stepBytes = contentLength; // tiny bodies
                                    nextKnownThreshold = stepBytes;
                                } else {
                                    // No intermediate progress for very small payloads
                                    nextKnownThreshold = long.MaxValue;
                                }
                            }

                            var chunk = await httpStream.ReadAsync(BUFFER_SIZE, InputStreamOptions.Partial);
                            if (!chunk.ReadSuccessful) {
                                break;
                            }
                            var len = chunk.Data.Length;
                            received += len;

                            if (contentLength > 0) {
                                // Known length: report at configured step boundaries.
                                while (stepsEmitted < totalSteps && received >= nextKnownThreshold) {
                                    // Reserve 100% for the final Finished event: cap progress at 99.
                                    var pct = (int)((100L * received) / contentLength);
                                    if (pct < 100) {
                                        //_logger.Debug("ParseRequestStream: " + pct + "% (" + received + "/" + contentLength + " bytes)");
                                        Report(HttpParseProgressStatus.InProgress, pct, received, contentLength, request.Uri);
                                    }

                                    stepsEmitted++;
                                    nextKnownThreshold = (stepsEmitted + 1) * stepBytes;
                                }
                            } else {
                                // Unknown length: report every N bytes (e.g., 1 MiB)
                                if (received >= nextUnknownThreshold) {
                                    //_logger.Debug("ParseRequestStream: " + received + " bytes (unknown length)");
                                    Report(HttpParseProgressStatus.InProgress, null, received, null, request.Uri);
                                    nextUnknownThreshold += _unknownLengthChunkBytes;
                                }
                            }

                            streamData = streamData.ConcatArray(chunk.Data);
                        }
                    }
                }

                request.IsComplete = requestPipeline.All(p => p.IsSucceeded);

                if (request.IsComplete) {
                    // We are also counting headers, so the total received length will be slightly more
                    // than the length of the content. To keep things simple, just clamp it.
                    if (received > contentLength) {
                        received = contentLength;
                    }

                    // Final 100% Finished event
                    var finalLen = contentLength > 0 ? (long?)contentLength : null;
                    Report(HttpParseProgressStatus.Finished, 100, received, finalLen, request.Uri);
                } else {
                    Report(HttpParseProgressStatus.Error, null, received, contentLength > 0 ? (long?)contentLength : null, request.Uri, "HTTP request parsing failed.");
                }

                //_logger.Debug($"[OperationController.TryRunOperationByRequestUri] URI:{request.Uri.ToString()}");

                OperationController.TryRunOperationByRequestUri(request);  //Execute functionality based on the requested URI.

                //if (request.Content != null) {
                //    _logger.Debug($"RequestContentLength: {request.Content.Length}");
                //    _logger.Debug("The byte[] data has been saved to requestContent.");
                //}
            } catch (Exception ex) {
                _logger.Debug(ex.Message);
                Report(HttpParseProgressStatus.Error, null, received, contentLength > 0 ? (long?)contentLength : null, request.Uri, ex.Message);
            }

            return request;
        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<IHttpRequestPartParser> GetPipeline() {
            return new IHttpRequestPartParser[]
            {
                    new MethodParser(),
                    new ResourceIdentifierParser(),
                    new ProtocolVersionParser(),
                    new HeadersParser(),
                    new ContentParser()
            };
        }

        private void Report(HttpParseProgressStatus status, int? pct, long received, long? contentLength, Uri uri, string error = null) {
            ProgressChanged?.Invoke(this, new HttpParseProgressEventArgs(status, pct, received, contentLength, uri, error));
        }

        private int StepCountFor(long contentLength) {
            if (contentLength <= 0) return 0;
            if (contentLength <= _smallFileBytesThreshold) return _smallFileSteps;
            if (contentLength <= _mediumFileBytesThreshold) return _mediumFileSteps;
            return _largeFileSteps;
        }

        #endregion Private Methods
    }
}