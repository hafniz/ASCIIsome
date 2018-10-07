using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ASCIIsome
{
    public class DisplayLanguage
    {
        public string DisplayName { get; set; }
        public string CultureSymbol { get; set; }
        public int Index { get; set; }
        public static List<DisplayLanguage> SupportedLanguage { get; } = new List<DisplayLanguage>
        {
            new DisplayLanguage("English", "en-US", 0),
            new DisplayLanguage("中文（简体）", "zh-CN", 1),
            new DisplayLanguage("русский", "ru-RU", 2)
        };

        public DisplayLanguage(string displayName, string cultureSymbol, int index)
        {
            DisplayName = displayName;
            CultureSymbol = cultureSymbol;
            Index = index;
        }

        public static void ChangeDisplayLanguage(ViewModel senderViewModel)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(senderViewModel.DisplayLanguage.CultureSymbol);
            (Application.Current.MainWindow as MainWindow)?.Close();
            new MainWindow().Show(senderViewModel);
            Application.Current.Windows.OfType<ChangeLanguage>().Single().Close();
        }

        public static DisplayLanguage GetDisplayLanguageFromSymbol(string symbol) => SupportedLanguage.Find(x => x.CultureSymbol == symbol);
        public static DisplayLanguage GetDisplayLanguageFromIndex(int index) => SupportedLanguage.Find(x => x.Index == index);

        public override string ToString() => DisplayName;
    }
}
