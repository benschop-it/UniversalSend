using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private ObservableCollection<SelectableReceiveItem> _files = new ObservableCollection<SelectableReceiveItem>();

        #endregion Private Fields

        #region Public Constructors

        public ConfirmReceiptViewModel(IReceiveManager receiveManager, INavigationService navigationService) {
            _receiveManager = receiveManager;
            _navigationService = navigationService;

            AcceptCommand = new RelayCommand(() => {
                ApplyAcceptedFiles();
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

        public ObservableCollection<SelectableReceiveItem> Files {
            get => _files;
            private set => Set(ref _files, value);
        }

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
            Files = new ObservableCollection<SelectableReceiveItem>(
                parameter.SendRequestData?.Files?.Values.Select(x => new SelectableReceiveItem {
                    FileId = x.Id,
                    FileName = x.FileName,
                    FileType = x.FileType,
                    Size = x.Size,
                    IsAccepted = true,
                }) ?? Enumerable.Empty<SelectableReceiveItem>());
        }

        public void Cancel(CancelReceiptPageParameter parameter) {
            IsCancelled = true;
            parameter?.CompletionSource?.TrySetResult(true);
        }

        private void ApplyAcceptedFiles() {
            if (SendRequestDataV2?.Files == null) {
                return;
            }

            var acceptedIds = new HashSet<string>(Files.Where(x => x.IsAccepted).Select(x => x.FileId));
            var acceptedFiles = SendRequestDataV2.Files
                .Where(x => acceptedIds.Contains(x.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            SendRequestDataV2.Files = acceptedFiles;
        }

        #endregion Public Methods

    }

    public class SelectableReceiveItem : INotifyPropertyChanged {

        #region Private Fields

        private bool _isAccepted;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string FileId { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public bool IsAccepted {
            get => _isAccepted;
            set => Set(ref _isAccepted, value);
        }

        public long Size { get; set; }

        public string Subtitle => string.IsNullOrWhiteSpace(FileType)
            ? $"{Size} bytes"
            : $"{FileType} • {Size} bytes";

        #endregion Public Properties

        #region Private Methods

        private bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        #endregion Private Methods
    }
}