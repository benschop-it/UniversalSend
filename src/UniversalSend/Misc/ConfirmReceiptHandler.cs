using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalSend.Models.Interfaces;
using UniversalSend.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UniversalSend.Misc {

    public class ConfirmReceiptHandler : IConfirmReceiptHandler {

        #region Public Methods

        public async Task<bool> ConfirmAsync(ISendRequestDataV2 sendRequest) {
            var tcs = new TaskCompletionSource<bool>();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => {
                    var frame = Window.Current.Content as Frame;
                    if (IsImmediateTextMessage(sendRequest)) {
                        frame?.Navigate(typeof(ReceivedTextPage), new ReceivedTextPageParameter {
                            SendRequestData = sendRequest,
                            CompletionSource = tcs
                        });
                    } else {
                        frame?.Navigate(typeof(ConfirmReceiptPage), new ConfirmReceiptPageParameter {
                            SendRequestData = sendRequest,
                            CompletionSource = tcs
                        });
                    }
                })
                .AsTask(); // <-- Fix here

            return await tcs.Task;
        }

        public async Task<bool> CancelAsync() {
            var tcs = new TaskCompletionSource<bool>();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => {
                    if (((ContentControl)Window.Current.Content).Content is RootPage15063) {
                        tcs.SetResult(false);
                    } else {
                        var frame = Window.Current.Content as Frame;
                        frame?.Navigate(typeof(ConfirmReceiptPage), new CancelReceiptPageParameter {
                            CompletionSource = tcs
                        });
                    }
                })
                .AsTask(); // <-- Fix here

            return await tcs.Task;
        }

        private static bool IsImmediateTextMessage(ISendRequestDataV2 sendRequest) {
            if (sendRequest?.Files == null || sendRequest.Files.Count != 1) {
                return false;
            }

            var file = sendRequest.Files.Values.FirstOrDefault();
            if (file == null) {
                return false;
            }

            if (!string.Equals(file.FileType, "text/plain", StringComparison.OrdinalIgnoreCase)) {
                return false;
            }

            if (string.IsNullOrEmpty(file.Preview)) {
                return false;
            }

            // LocalSend message transfers inline the full message in preview and do not require an upload phase.
            // Treat it as a message only when the announced size matches the preview payload size.
            return file.Size == Encoding.UTF8.GetByteCount(file.Preview);
        }

        #endregion Public Methods
    }

    public class ConfirmReceiptPageParameter {

        #region Public Properties

        public TaskCompletionSource<bool> CompletionSource { get; set; }
        public ISendRequestDataV2 SendRequestData { get; set; }

        #endregion Public Properties
    }

    public class CancelReceiptPageParameter {

        #region Public Properties

        public TaskCompletionSource<bool> CompletionSource { get; set; }

        #endregion Public Properties
    }

    public class ReceivedTextPageParameter {

        #region Public Properties

        public TaskCompletionSource<bool> CompletionSource { get; set; }

        public ISendRequestDataV2 SendRequestData { get; set; }

        #endregion Public Properties
    }
}