using UniversalSend.Services.HttpMessage.Headers;
using UniversalSend.Services.HttpMessage.Headers.Request;

namespace UniversalSend.Services.HttpMessage.Models.Contracts {

    public interface IHttpRequestHeaderVisitor<T> {

        void Visit(UntypedRequestHeader uh, T arg);

        void Visit(ContentLengthHeader uh, T arg);

        void Visit(AcceptHeader uh, T arg);

        void Visit(ContentTypeHeader uh, T arg);

        void Visit(AcceptCharsetHeader uh, T arg);

        void Visit(AcceptEncodingHeader uh, T arg);

        void Visit(AccessControlRequestMethodHeader uh, T arg);

        void Visit(AccessControlRequestHeadersHeader uh, T arg);

        void Visit(OriginHeader uh, T arg);
    }
}