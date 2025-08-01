using UniversalSend.Services.Models.Schemas;
using UniversalSend.Services.Rest.Models.Contracts;

namespace UniversalSend.Services.Rest {

    internal class RestResponseFactory {

        #region Private Fields

        private readonly BadRequestResponse _badRequestResponse;

        #endregion Private Fields

        #region Internal Constructors

        internal RestResponseFactory() {
            _badRequestResponse = new BadRequestResponse();
        }

        #endregion Internal Constructors

        #region Internal Methods

        internal IRestResponse CreateBadRequest() {
            return _badRequestResponse;
        }

        #endregion Internal Methods
    }
}