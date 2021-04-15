using Cryptotracker.Languages;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Cryptotracker.Frontend.Converters
{
    public class LanguageToCultureInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is not Language lang ? null : lang switch
            {
                Language.Polski => new CultureInfo("pl-PL"),
                _ => new CultureInfo("en-US")
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
