using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof((string, string)), typeof(string))]
    public sealed class CharSetNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (((string displayName, string filename))value).displayName;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
