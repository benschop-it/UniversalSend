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

namespace UniversalSend.Controls.SendPage
{
    public sealed partial class SendItemButtonControl : UserControl
    {
        public string IconGlyph { get; set; }

        public string Label { get; set; }
        public SendItemButtonControl(string iconGlyph, string label)
        {
            this.InitializeComponent();
            IconGlyph = iconGlyph;
            Label = label;
        }
    }
}
