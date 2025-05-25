using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UniversalSend.Models
{
    public class UIManager
    {
        public static Thickness RootElementMargin { get; set; } = new Thickness(24);
        public static Thickness RootElementMarginWithoutTop { get; set; } = new Thickness(24,0,24,24);

        public static void InitRootElementMargin()
        {
            if(SystemHelper.GetDeviceFormFactorType() == SystemHelper.DeviceFormFactorType.Phone)
            {
                RootElementMargin = new Thickness(12);
                RootElementMarginWithoutTop = new Thickness(12,0,12,12);
            }
        }
    }
}
