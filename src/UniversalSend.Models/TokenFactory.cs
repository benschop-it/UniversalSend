using System;

namespace UniversalSend.Models {

    public class TokenFactory {

        #region Public Methods

        public static string CreateToken() {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        #endregion Public Methods
    }
}