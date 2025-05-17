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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace UniversalSend.Controls.ContentDialogControls
{
    public sealed partial class ManualSendControl : UserControl
    {
        public ManualSendControl()
        {
            this.InitializeComponent();
        }

        public int Mode = 0;

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mode = MainPivot.SelectedIndex;
        }
    }
}
