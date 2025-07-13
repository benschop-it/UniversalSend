using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace UniversalSend.Models {
    public class ColorHelper {
        public static string ConvertColorToArgbString(Color color) {
            // Use String.Format to format ARGB values as a hexadecimal string
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
        }

        public static Color ConvertArgbStringToColor(string argbString) {
            // Remove the '#' prefix from the string
            if (argbString.StartsWith("#")) {
                argbString = argbString.Substring(1);
            }

            // Ensure the string length is 8 (each ARGB component is 2 hex digits)
            if (argbString.Length != 8) {
                throw new ArgumentException("Invalid ARGB string format.");
            }

            // Parse hexadecimal values
            byte a = byte.Parse(argbString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(argbString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(argbString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(argbString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            // Create and return a Windows.UI.Color object
            return Color.FromArgb(a, r, g, b);
        }
    }
}
