using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// For more information on the "UserControl" item template, see https://go.microsoft.com/fwlink/?LinkId=234236

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class CreateNewFolderControl : UserControl
    {
        public CreateNewFolderControl()
        {
            this.InitializeComponent();
        }

        private void NameTextBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((TextBox)sender).Focus(FocusState.Pointer);
        }
    }
}
