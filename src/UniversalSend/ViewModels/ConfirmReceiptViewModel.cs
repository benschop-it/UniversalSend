using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using UniversalSend.Misc;
using UniversalSend.Models.Interfaces;

namespace UniversalSend.Interfaces {

    public class ConfirmReceiptViewModel : ViewModelBase, IConfirmReceiptViewModel {

        #region Private Fields

        private readonly INavigationService _navigationService;
        private readonly IReceiveManager _receiveManager;
        private ISendRequestDataV2 _sendRequestDataV2;

        #endregion Private Fields

        #region Public Constructors

        public ConfirmReceiptViewModel(IReceiveManager receiveManager, INavigationService navigationService) {
            _receiveManager = receiveManager;
            _navigationService = navigationService;

            AcceptCommand = new RelayCommand(() => {
                Parameter?.CompletionSource?.TrySetResult(true);
                _navigationService.GoBack();
            });

            CancelCommand = new RelayCommand(() => {
                Parameter?.CompletionSource?.TrySetResult(false);
                _navigationService.GoBack();
            });
        }

        #endregion Public Constructors

        #region Public Properties

        public RelayCommand AcceptCommand { get; }

        public RelayCommand CancelCommand { get; }

        public ConfirmReceiptPageParameter Parameter { get; private set; }

        public ISendRequestDataV2 SendRequestDataV2 {
            get => _sendRequestDataV2;
            private set => Set(ref _sendRequestDataV2, value);
        }

        #endregion Public Properties

        #region Public Methods

        public void Initialize(ConfirmReceiptPageParameter parameter) {
            Parameter = parameter;
            SendRequestDataV2 = parameter.SendRequestData;
        }

        #endregion Public Methods

    }
}