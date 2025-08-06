using System;
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

        #endregion Public Methods
    }

    public class ConfirmReceiptPageParameter {

        #region Public Properties

        public TaskCompletionSource<bool> CompletionSource { get; set; }
        public ISendRequestDataV2 SendRequestData { get; set; }

        #endregion Public Properties
    }
}