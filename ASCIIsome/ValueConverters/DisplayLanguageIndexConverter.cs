using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof(DisplayLanguage), typeof(int))]
    public sealed class DisplayLanguageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as DisplayLanguage)?.Index;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value != null ? DisplayLanguage.GetDisplayLanguageFromIndex((int)value) : null;
    }
}
