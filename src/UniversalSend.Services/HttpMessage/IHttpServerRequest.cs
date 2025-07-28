using System;
using System.Collections.Generic;
using UniversalSend.Services.HttpMessage.Models.Contracts;
using UniversalSend.Services.HttpMessage.Models.Schemas;

namespace UniversalSend.Services.HttpMessage
{
    public interface IHttpServerRequest
    {
        IEnumerable<IHttpRequestHeader> Headers { get; }
        HttpMethod? Method { get;  }
        Uri Uri { get;  }
        string HttpVersion { get;  }
        string ContentTypeCharset { get;  }
        IEnumerable<string> AcceptCharsets { get;  }
        int ContentLength { get;  }
        string ContentType { get;  }
        IEnumerable<string> AcceptEncodings { get; }
        IEnumerable<string> AcceptMediaTypes { get;  }
        byte[] Content { get;  }
        bool IsComplete { get;  }
        HttpMethod? AccessControlRequestMethod { get; }
        IEnumerable<string> AccessControlRequestHeaders { get; }
        string Origin { get; }
    }
}