using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

/*
 * XAML example of use:
 * 
 * <Converters:ConverterChain x:Key="NegatedBoolToVisibilityConverter">
 *      <Converters:BoolNegatorConverter/>
 *      <BooleanToVisibilityConverter/>
 *  </Converters:ConverterChain>
 */
namespace Cryptotracker.Frontend.Converters
{
    /// <summary>
    /// Represents a chain of <see cref="IValueConverter"/>s to be executed in succession.
    /// </summary>
    [ContentProperty(nameof(Converters))]
    [ContentWrapper(typeof(ValueConverterCollection))]
    public class ConverterChain : IValueConverter
    {
        private readonly ValueConverterCollection _converters = new();

        public ValueConverterCollection Converters => _converters;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters.Aggregate(value, (currentValue, conv) => conv.Convert(currentValue, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters.Reverse().Aggregate(value, (curr, conv) => conv.Convert(curr, targetType, parameter, culture));
        }
    }

    /// <summary>Represents a collection of <see cref="IValueConverter"/>s.</summary>
    public sealed class ValueConverterCollection : Collection<IValueConverter> { }
}
