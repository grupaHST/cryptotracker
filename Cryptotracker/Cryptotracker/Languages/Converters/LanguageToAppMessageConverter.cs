using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Cryptotracker.Languages.Converters
{
    public class LanguageToAppMessageConverter : IValueConverter
    {
        /// <summary>
        /// Zwraca rezultat wywołania odpowiedniej funkcji z klasy <see cref="AppMessages"/>.
        /// </summary>
        /// <param name="value">Język zwracanej metody z enuma <see cref="Language"/></param>
        /// <param name="parameter">Nazwa funkcji z klasy <see cref="AppMessages"/></param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Language lang && parameter is string appMessage)
            {
                var func = typeof(AppMessages).GetMethods().FirstOrDefault(x => x.Name == appMessage);
                return func?.Invoke(null, new object[] { lang })?.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
