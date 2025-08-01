namespace UniversalSend.Services.HttpMessage.Models.Schemas {

    internal struct ExtractedWord {

        #region Public Properties

        public byte[] RemainingBytes { get; set; }
        public string Word { get; set; }
        public bool WordFound { get; set; }

        #endregion Public Properties
    }
}