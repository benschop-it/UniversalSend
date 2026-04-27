using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UniversalSend.Models.Interfaces;
using Windows.Storage;

namespace UniversalSend.Models.Tasks {

    internal class SendTaskV2 : ISendTaskV2, INotifyPropertyChanged {

        #region Private Fields

        private IUniversalSendFileV2 _file;
        private string _sessionId;
        private IStorageFile _storageFile;
        private ReceiveTaskStates _taskState = ReceiveTaskStates.Waiting;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public IUniversalSendFileV2 File {
            get => _file;
            set => Set(ref _file, value);
        }

        public IStorageFile StorageFile {
            get => _storageFile;
            set => Set(ref _storageFile, value);
        }

        public ReceiveTaskStates TaskState {
            get => _taskState;
            set => Set(ref _taskState, value);
        }

        public string SessionId {
            get => _sessionId;
            set => Set(ref _sessionId, value);
        }

        #endregion Public Properties

        #region Private Methods

        private bool Set<T>(ref T field, T value, [CallerMemberName] string name = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }

        #endregion Private Methods

    }
}