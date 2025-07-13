using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UniversalSend.Models;
using UniversalSend.Models.Data;
using UniversalSend.Models.HttpData;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// For more information on the "Blank Page" item template, see https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalSend.Views
{
    /// <summary>
    /// A blank page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfirmReceiptPage : Page
    {
        SendRequestData SendRequestData { get; set; }

        public ConfirmReceiptPage()
        {
            this.InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ReceiveManager.ChosenOption = false;
            Frame.GoBack();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ReceiveManager.ChosenOption = true;
            Frame.GoBack();
        }
    }
}
