using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Models
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //if (value is Enum enumValue)
            //{
            //    var field = enumValue.GetType().GetField(enumValue.ToString());
            //    var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //    if (attributes.Length > 0)
            //    {
            //        return ((DescriptionAttribute)attributes[0]).Description;
            //    }
            //}
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
