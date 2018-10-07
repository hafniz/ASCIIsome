using System;
using System.Globalization;
using System.Windows.Data;

namespace ASCIIsome.ValueConverters
{
    [ValueConversion(typeof(DisplayLanguage), typeof(int))]
    public class DisplayLanguageIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as DisplayLanguage).Index;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DisplayLanguage.GetDisplayLanguageFromIndex((int)value);
    }
}
