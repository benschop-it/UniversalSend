using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UniversalSend.Models.HttpData;
using UniversalSend.Models.Interfaces;
using UniversalSend.Models.Tasks;
using Windows.Storage;

namespace UniversalSend.Models.Managers {
    internal class ReceiveTaskManager : IReceiveTaskManager {

        private IStorageHelper _storageHelper;

        public ReceiveTaskManager(IStorageHelper storageHelper) {
            _storageHelper = storageHelper ?? throw new ArgumentNullException(nameof(storageHelper));
        }

        #region Public Properties

        public List<IReceiveTask> ReceivingTasks { get; private set; } = new List<IReceiveTask>();

        #endregion Public Properties

        #region Public Methods

        public void CreateReceivingTaskFromUniversalSendFile(IUniversalSendFile universalSendFile, IInfoData info) {
            IReceiveTask receiveTask = new ReceiveTask { File = universalSendFile, Sender = info };
            ReceivingTasks.Add(receiveTask);
        }

        public IReceiveTask WriteFileContentToReceivingTask(string fileId, string token, byte[] fileContent) {
            IReceiveTask task = ReceivingTasks.Find(x => x.File.Id == fileId);
            if (task == null || task.File.TransferToken != token) {
                // task.TaskState = ReceiveTask.ReceiveTaskStates.Error;
                return null;
            }
            task.FileContent = fileContent;
            task.TaskState = ReceiveTaskStates.Done;
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