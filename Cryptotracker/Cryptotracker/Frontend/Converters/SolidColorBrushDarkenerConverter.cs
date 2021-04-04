using static System.Convert;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Cryptotracker.Frontend.Converters
{
    public class SolidColorBrushDarkenerConverter : IValueConverter
    {
        public static readonly int DarkenerLevel = 0x40;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is not SolidColorBrush brush ? null : new SolidColorBrush(Color.FromRgb
        (
            ToByte(Math.Clamp(brush.Color.R - DarkenerLevel, byte.MinValue, byte.MaxValue)),
            ToByte(Math.Clamp(brush.Color.G - DarkenerLevel, byte.MinValue, byte.MaxValue)),
            ToByte(Math.Clamp(brush.Color.B - DarkenerLevel, byte.MinValue, byte.MaxValue))
        ));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is not SolidColorBrush brush ? null : new SolidColorBrush(Color.FromRgb
        (
            ToByte(Math.Clamp(DarkenerLevel + brush.Color.R, byte.MinValue, byte.MaxValue)),
            ToByte(Math.Clamp(DarkenerLevel + brush.Color.G, byte.MinValue, byte.MaxValue)),
            ToByte(Math.Clamp(DarkenerLevel + brush.Color.B, byte.MinValue, byte.MaxValue))
        ));
    }
}
