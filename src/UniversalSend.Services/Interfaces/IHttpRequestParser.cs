using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Services.HttpMessage;
using Windows.Storage.Streams;

namespace UniversalSend.Services.Interfaces {
    public interface IHttpRequestParser {
        Task<IMutableHttpServerRequest> ParseRequestStream(IInputStream requestStream);
    }
}
