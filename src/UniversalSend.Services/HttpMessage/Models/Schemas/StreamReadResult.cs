namespace UniversalSend.Services.HttpMessage.Models.Schemas {

    internal struct StreamReadResult {

        #region Internal Constructors

        internal StreamReadResult(byte[] data, bool successful) {
            Data = data;
            ReadSuccessful = successful;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal byte[] Data { get; }
        internal bool ReadSuccessful { get; }

        #endregion Internal Properties
    }
}