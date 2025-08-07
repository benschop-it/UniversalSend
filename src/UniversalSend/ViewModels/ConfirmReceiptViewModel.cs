using System;
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
        private bool _isCancelled;

        #endregion Private Fields

        #region Public Constructors

        public ConfirmReceiptViewModel(IReceiveManager receiveManager, INavigationService navigationService) {
            _receiveManager = receiveManager;
            _navigationService = navigationService;

            AcceptCommand = new RelayCommand(() => {
                ConfirmReceiptPageParameter?.CompletionSource?.TrySetResult(true);
                _navigationService.GoBack();
            });

            CancelCommand = new RelayCommand(() => {
                ConfirmReceiptPageParameter?.CompletionSource?.TrySetResult(false);
                _navigationService.GoBack();
            });

            CloseCommand = new RelayCommand(() =>
            {
                IsCancelled = false;
                _navigationService.GoBack();

                ConfirmReceiptPageParameter?.CompletionSource?.TrySetResult(false);
                _navigationService.GoBack();
            });

            ShowOptionsCommand = new RelayCommand(OnShowOptions);
        }

        private void OnShowOptions() {
        }

        #endregion Public Constructors

        #region Public Properties

        public RelayCommand AcceptCommand { get; }

        public RelayCommand CancelCommand { get; }

        public RelayCommand ShowOptionsCommand { get; }

        public RelayCommand CloseCommand { get; }

        public ConfirmReceiptPageParameter ConfirmReceiptPageParameter { get; private set; }

        public ISendRequestDataV2 SendRequestDataV2 {
            get => _sendRequestDataV2;
            private set {
                Set(ref _sendRequestDataV2, value);
                RaisePropertyChanged(nameof(Host));
            }
        }

        public string Host {
            get {
                var fullHost = _sendRequestDataV2?.Host;
                if (string.IsNullOrEmpty(fullHost)) return string.Empty;

                var hostPart = fullHost.Split(':')[0]; // remove port
                if (System.Net.IPAddress.TryParse(hostPart, out var ip)) {
                    var octets = hostPart.Split('.');
                    return "#" + octets[octets.Length - 1]; // return last octet
                }
                return hostPart; // hostname, e.g., 'somehost.local'
            }
        }

        public bool IsCancelled {
            get => _isCancelled;
            private set => Set(ref _isCancelled, value);
        }

        #endregion Public Properties

        #region Public Methods

        public void Initialize(ConfirmReceiptPageParameter parameter) {
            ConfirmReceiptPageParameter = parameter;
            SendRequestDataV2 = parameter.SendRequestData;
        }

        public void Cancel(CancelReceiptPageParameter parameter) {
            IsCancelled = true;
            parameter?.CompletionSource?.TrySetResult(true);
        }

        #endregion Public Methods

    }
}