using System;
using System.Diagnostics;
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
                    frame?.Navigate(typeof(ConfirmReceiptPage), new ConfirmReceiptPageParameter {
                        SendRequestData = sendRequest,
                        CompletionSource = tcs
                    });
                })
                .AsTask(); // <-- Fix here

            return await tcs.Task;
        }

        public async Task<bool> CancelAsync() {
            bool isProcess = false;
            var tcs = new TaskCompletionSource<bool>();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher
                .RunAsync(CoreDispatcherPriority.Normal, () => {
                    if (((ContentControl)Window.Current.Content).Content is RootPage15063) {
                        Debug.WriteLine("On the root page!");
                        tcs.SetResult(false);
                    } else {
                        Debug.WriteLine($"On the {((ContentControl)Window.Current.Content).Content.GetType().FullName} page!");
                        var frame = Window.Current.Content as Frame;
                        frame?.Navigate(typeof(ConfirmReceiptPage), new CancelReceiptPageParameter {
                            CompletionSource = tcs
                        });
                    }
                })
                .AsTask(); // <-- Fix here

            return await tcs.Task;
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
}