using System.Threading.Tasks;

namespace UniversalSend.Models.Interfaces {

    public interface IConfirmReceiptHandler {

        #region Public Methods

        /// <summary>
        /// Asks the user to confirm an incoming send request.
        /// </summary>
        /// <param name="sendRequest">The incoming send request data.</param>
        /// <returns>True if accepted, false if rejected.</returns>
        Task<bool> ConfirmAsync(ISendRequestDataV2 sendRequest);

        Task<bool> CancelAsync();

        #endregion Public Methods
    }
}