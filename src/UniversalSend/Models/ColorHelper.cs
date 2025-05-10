using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace UniversalSend.Models
{
    public class ColorHelper
    {
        public static string ConvertColorToArgbString(Color color)
        {
            // 使用String.Format方法将ARGB值格式化为十六进制字符串
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static Color ConvertArgbStringToColor(string argbString)
        {
            // 移除字符串开头的'#'
            if (argbString.StartsWith("#"))
            {
                argbString = argbString.Substring(1);
            }

            // 确保字符串长度为8（ARGB各占2位十六进制）
            if (argbString.Length != 8)
            {
                throw new ArgumentException("Invalid ARGB string format.");
            }

            // 解析十六进制值
            byte a = byte.Parse(argbString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(argbString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(argbString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(argbString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            // 创建并返回Windows.UI.Color对象
            return Color.FromArgb(a, r, g, b);
        }
    }
}
