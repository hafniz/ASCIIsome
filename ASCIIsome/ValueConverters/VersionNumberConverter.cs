using System;
using System.Globalization;
using System.Windows.Data;
using static ASCIIsome.ApplicationInfo;

namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof(Version), typeof(string))]
    public class VersionNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => VersionPrefix + value.ToString() + VersionSuffix;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
