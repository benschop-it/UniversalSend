using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;
using Windows.Storage.Streams;

namespace UniversalSend.Services.Interfaces {
    public interface IMutableHttpServerRequest {
        IEnumerable<string> AcceptCharsets { get; set; }
        IEnumerable<string> AcceptEncodings { get; set; }
        IEnumerable<string> AcceptMediaTypes { get; set; }
        IEnumerable<string> AccessControlRequestHeaders { get; set; }
        HttpMethod? AccessControlRequestMethod { get; set; }
        byte[] Content { get; set; }
        int ContentLength { get; set; }
        string ContentType { get; set; }
        string ContentTypeCharset { get; set; }
        //IEnumerable<IHttpRequestHeader> Headers { get; }
        string HttpVersion { get; set; }
        bool IsComplete { get; set; }
        HttpMethod? Method { get; set; }
        string Origin { get; set; }
        Uri Uri { get; set; }
        Task<IMutableHttpServerRequest> Parse(IInputStream requestStream);
    }
}