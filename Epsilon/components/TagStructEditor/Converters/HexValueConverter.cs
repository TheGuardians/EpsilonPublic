using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TagStructEditor.Converters
{
    public class HexValueConverter : IValueConverter
    {
        public static HexValueConverter Instance = new HexValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // convert from byte[] 

            var bytes = (byte[])value;
            if (bytes == null)
                return DependencyProperty.UnsetValue;

            var buff = new StringBuilder(bytes.Length * 2);

            char f(int x) => (char)(x < 10 ? ('0' + x) : ('A' + (x - 10)));

            for (int i = 0; i < bytes.Length; i++)
            {
                buff.Append(f(bytes[i] >> 4));
                buff.Append(f(bytes[i] & 0xf));
            }

            return buff.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // convert to byte[]

            var hex = value as string;
            if (hex == null)
                return null;

            if (hex.Length % 2 != 0)
                return DependencyProperty.UnsetValue;

            byte f(char x) => (x >= 'A' && x <= 'F') ? (byte)(x - 'A' + 10) : (byte)(x - '0');

            var bytes = new byte[hex.Length / 2];
            for (int i = 0, j = 0; i < hex.Length; i += 2)
            {
                if(!ValidHexChar(hex[i]) || !ValidHexChar(hex[i]))
                    return DependencyProperty.UnsetValue;

                byte hi = f(hex[i]);
                byte lo = f(hex[i + 1]);
                bytes[j++] = (byte)((hi << 4) | lo);
            }

            return bytes;
        }

        private static bool ValidHexChar(char c)
        {
            return c >= '0' && c <= '9' || c >= 'A' && c <= 'F';
        }
    }
}
