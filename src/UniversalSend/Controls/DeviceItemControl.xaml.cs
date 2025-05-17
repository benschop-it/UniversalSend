using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UniversalSend.Models.Data;
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

namespace UniversalSend.Controls
{
    public sealed partial class DeviceItemControl : UserControl
    {
        Device Device { get; set; }
        public DeviceItemControl(Device device)
        {
            this.InitializeComponent();
            this.Device = device;
            
            HashTagTextBlock.Text = $"#{device.IP.Substring(device.IP.LastIndexOf(".")+1)}";

        }
    }
}
