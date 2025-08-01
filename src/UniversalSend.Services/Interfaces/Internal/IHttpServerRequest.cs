using System;
using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpServerRequest {

        #region Public Properties

        IEnumerable<string> AcceptCharsets { get; }
        IEnumerable<string> AcceptEncodings { get; }
        IEnumerable<string> AcceptMediaTypes { get; }
        IEnumerable<string> AccessControlRequestHeaders { get; }
        HttpMethod? AccessControlRequestMethod { get; }
        byte[] Content { get; }
        int ContentLength { get; }
        string ContentType { get; }
        string ContentTypeCharset { get; }
        IEnumerable<IHttpRequestHeader> Headers { get; }
        string HttpVersion { get; }
        bool IsComplete { get; }
        HttpMethod? Method { get; }
        string Origin { get; }
        Uri Uri { get; }

        #endregion Public Properties

    }
}