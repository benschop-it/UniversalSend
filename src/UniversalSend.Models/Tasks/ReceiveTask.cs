using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UniversalSend.Models.Interfaces;
using UniversalSend.Services.Interfaces;

namespace UniversalSend.Models.Tasks {

    internal class ReceiveTask : IReceiveTask {

        #region Private Fields

        private int _progress;
        private string _error;
        private HttpParseProgressStatus _status;
        private ReceiveTaskStates _taskState = ReceiveTaskStates.Waiting;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public string Error {
            get => _error;
            set => Set(ref _error, value);
        }

        public IUniversalSendFileV1 FileV1 { get; set; }

        public IUniversalSendFileV2 FileV2 { get; set; }

        public byte[] FileContent { get; set; }

        public int Progress {
            get => _progress;
            set => Set(ref _progress, value);
        }

        // Generic notifier by name
        private void Notify([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Explicit by-name refresh
        public void Refresh(string propertyName) {
            Notify(propertyName);
        }

        // Optional: raise for multiple properties in one go
        public void Refresh(params string[] propertyNames) {
            foreach (var p in propertyNames) Notify(p);
        }

        public IInfoDataV1 SenderV1 { get; set; }

        public IInfoDataV2 SenderV2 { get; set; }

        public HttpParseProgressStatus Status {
            get { return _status; }
            set {
                _status = value;
            }
        }

        public ReceiveTaskStates TaskState {
            get => _taskState;
            set => Set(ref _taskState, value);
        }

        #endregion Public Properties

        #region Private Methods

        private bool Set<T>(ref T field, T value, [CallerMemberName] string name = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }

        #endregion Private Methods
    }
}