using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Rest {

    internal class RestResponseFactory {
        private readonly BadRequestResponse _badRequestResponse;

        internal RestResponseFactory() {
            _badRequestResponse = new BadRequestResponse();
        }

        internal IRestResponse CreateBadRequest() {
            return _badRequestResponse;
        }
    }
}