using System;
using System.Globalization;
using System.Windows.Data;

namespace Cryptotracker.Frontend.Converters
{
    public class SolidColorBrushLightenerConverter : IValueConverter
    {
        private readonly SolidColorBrushDarkenerConverter _darkener = new();

        public static readonly int LightenerLevel = SolidColorBrushDarkenerConverter.DarkenerLevel;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => _darkener.ConvertBack(value, targetType, parameter, culture);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => _darkener.Convert(value, targetType, parameter, culture);
    }
}
