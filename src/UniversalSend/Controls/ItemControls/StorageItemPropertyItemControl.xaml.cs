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

namespace UniversalSend.Controls.ItemControls
{
    public sealed partial class StorageItemPropertyItemControl : UserControl
    {
        string TitleText { get; set; }
        string ContentText { get; set; }
        public StorageItemPropertyItemControl(string titleText, string contentText)
        {
            this.InitializeComponent();
            TitleText = titleText;
            ContentText = contentText;
        }
    }
}
