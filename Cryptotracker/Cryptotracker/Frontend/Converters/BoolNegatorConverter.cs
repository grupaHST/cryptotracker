using System;
using System.Globalization;
using System.Windows.Data;

namespace Cryptotracker.Frontend.Converters
{
    public class BoolNegatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolean ? !boolean : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}
