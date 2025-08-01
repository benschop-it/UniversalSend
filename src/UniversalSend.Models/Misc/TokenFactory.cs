using System;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Models.Misc {

    internal class TokenFactory : ITokenFactory {

        #region Public Methods

        public string CreateToken() {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        #endregion Public Methods

    }
}