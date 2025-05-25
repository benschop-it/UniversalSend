using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace UniversalSend.Models
{
    public class LocalizeManager
    {
        public static string GetLocalizedString(string uid)
        {
            var resourceLoader = ResourceLoader.GetForCurrentView();
            var currentLanguage = resourceLoader.GetString("CurrentLanguage");
            return resourceLoader.GetString(uid);
        }
    }
}
