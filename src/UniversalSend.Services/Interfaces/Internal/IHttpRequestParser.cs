using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace UniversalSend.Services.Interfaces.Internal {

    internal interface IHttpRequestParser {

        #region Public Methods

        Task<IMutableHttpServerRequest> ParseRequestStream(IInputStream requestStream);

        #endregion Public Methods
    }
}