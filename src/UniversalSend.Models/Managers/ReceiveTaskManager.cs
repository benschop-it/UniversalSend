using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Tasks;
using Windows.Storage;
using Windows.UI.Core;

namespace UniversalSend.Models.Managers {

    internal class ReceiveTaskManager : IReceiveTaskManager {

        #region Private Fields

        private readonly IStorageHelper _storageHelper;
        private readonly CoreDispatcher _dispatcher;

        #endregion Private Fields

        #region Public Constructors

        public ReceiveTaskManager(IStorageHelper storageHelper, IDispatcherProvider dispatcherProvider) {
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
            _dispatcher = dispatcherProvider?.Dispatcher ?? throw new ArgumentNullException(nameof(dispatcherProvider));
        }

        #endregion Public Constructors

        #region Public Properties

        //public List<IReceiveTask> ReceivingTasks { get; private set; } = new List<IReceiveTask>();

        public ObservableCollection<IReceiveTask> ReceivingTasks { get; } = new ObservableCollection<IReceiveTask>();

        #endregion Public Properties

        #region Public Methods

        public async Task CreateReceivingTaskFromUniversalSendFileAsync(IUniversalSendFile file, IInfoData info) {
            var task = new ReceiveTask { File = file, Sender = info };

            if (_dispatcher.HasThreadAccess) {
                ReceivingTasks.Add(task);
            } else {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ReceivingTasks.Add(task));
            }
        }

        public void CreateReceivingTaskFromUniversalSendFile(IUniversalSendFile universalSendFile, IInfoData info) {
            IReceiveTask receiveTask = new ReceiveTask { File = universalSendFile, Sender = info };
            ReceivingTasks.Add(receiveTask);
        }

        public async Task<IReceiveTask> WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent) {
            IReceiveTask task = ReceivingTasks.FirstOrDefault(x =>
                x.File != null &&
                string.Equals(x.File.Id, fileId, StringComparison.Ordinal) &&
                string.Equals(x.File.TransferToken, token, StringComparison.Ordinal));

            if (task == null || task.File.TransferToken != token) {
                // task.TaskState = ReceiveTask.ReceiveTaskStates.Error;
                return null;
            }
            task.FileContent = fileContent;

            if (_dispatcher.HasThreadAccess) {
                task.TaskState = ReceiveTaskStates.Done;
            } else {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => task.TaskState = ReceiveTaskStates.Done);
            }

            return task;
        }

        public async Task<IStorageFile> WriteReceiveTaskToFileAsync(IReceiveTask receiveTask) {
            StorageFile storageFile;
            StorageFolder folder = await _storageHelper.GetReceiveStorageFolderAsync();
            if (folder == null) {
                storageFile = await _storageHelper.CreateFileInDownloadsFolderAsync(receiveTask.File.FileName);
            } else {
                storageFile = await folder.CreateFileAsync(receiveTask.File.FileName, CreationCollisionOption.GenerateUniqueName);
            }
            await _storageHelper.WriteBytesToFileAsync(storageFile, receiveTask.FileContent);
            return storageFile;
        }

        #endregion Public Methods

    }
}