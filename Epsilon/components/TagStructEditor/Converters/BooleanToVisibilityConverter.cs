using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TagStructEditor.Converters
{
    public class BoolToVisbilityConverter : IValueConverter
    {
        public static BoolToVisbilityConverter Instance = new BoolToVisbilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool inverted = parameter != null;
            return (bool)value != inverted ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
