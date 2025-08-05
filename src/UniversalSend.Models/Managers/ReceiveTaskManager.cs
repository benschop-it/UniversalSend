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

        public ObservableCollection<IReceiveTask> ReceivingTasks { get; } = new ObservableCollection<IReceiveTask>();

        #endregion Public Properties

        #region Public Methods

        public async Task CreateReceivingTaskFromUniversalSendFileV2Async(IUniversalSendFileV2 file, IInfoDataV2 info) {
            var task = new ReceiveTask { FileV2 = file, SenderV2 = info };

            if (_dispatcher.HasThreadAccess) {
                ReceivingTasks.Add(task);
            } else {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => ReceivingTasks.Add(task));
            }
        }

        public void CreateReceivingTaskFromUniversalSendFileV2(IUniversalSendFileV2 universalSendFile, IInfoDataV2 info) {
            IReceiveTask receiveTask = new ReceiveTask { FileV2 = universalSendFile, SenderV2 = info };
            ReceivingTasks.Add(receiveTask);
        }

        public async Task<IReceiveTask> WriteFileContentToReceivingTaskV2(string sessionId, string fileId, string token, byte[] fileContent) {
            //TODO: do something with the session Id.

            IReceiveTask task = ReceivingTasks.FirstOrDefault(x =>
                x.FileV2 != null &&
                string.Equals(x.FileV2.Id, fileId, StringComparison.Ordinal) &&
                string.Equals(x.FileV2.TransferToken, token, StringComparison.Ordinal));

            if (task == null || task.FileV2.TransferToken != token) {
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

        public async Task<IStorageFile> WriteReceiveTaskToFileV2Async(IReceiveTask receiveTask) {
            StorageFile storageFile;
            StorageFolder folder = await _storageHelper.GetReceiveStorageFolderAsync();
            if (folder == null) {
                storageFile = await _storageHelper.CreateFileInDownloadsFolderAsync(receiveTask.FileV2.FileName);
            } else {
                storageFile = await folder.CreateFileAsync(receiveTask.FileV2.FileName, CreationCollisionOption.GenerateUniqueName);
            }
            await _storageHelper.WriteBytesToFileAsync(storageFile, receiveTask.FileContent);
            return storageFile;
        }


        #endregion Public Methods

    }
}