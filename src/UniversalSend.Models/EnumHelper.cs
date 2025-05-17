using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UniversalSend.Models
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Enum enumValue)
            {
                MemberInfo[] memberInfo = value.GetType().GetMember(enumValue.ToString());

                if (memberInfo != null && memberInfo.Length > 0)
                {
                    // 尝试获取 Description 特性
                    var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Count() > 0)
                    {
                        // 返回 Description 特性的值
                        return ((DescriptionAttribute)attrs.ToList()[0]).Description;
                    }
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
