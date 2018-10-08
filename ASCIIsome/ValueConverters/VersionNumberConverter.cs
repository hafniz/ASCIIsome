using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof(Version), typeof(string))]
    public class VersionNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => "v" + value.ToString();
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => new Version((value as string).Remove(0, 1));
    }
}
