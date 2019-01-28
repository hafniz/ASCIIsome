using System;
using System.Globalization;
using System.Windows.Data;
using static ASCIIsome.ApplicationInfo;

#nullable enable
namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof(Version), typeof(string))]
    public sealed class VersionNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => VersionPrefix + value + VersionSuffix;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
