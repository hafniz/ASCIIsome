using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ASCIIsome.Windows;

namespace ASCIIsome
{
    public class DisplayLanguage
    {
        public string DisplayName { get; set; }
        public string CultureSymbol { get; set; }
        public int Index { get; set; }

        public static DisplayLanguage GetDisplayLanguageFromSymbol(string symbol) => SupportedLanguage.Find(x => x.CultureSymbol == symbol);
        public static DisplayLanguage GetDisplayLanguageFromIndex(int index) => SupportedLanguage.Find(x => x.Index == index);
        public override string ToString() => DisplayName;

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

        public static void ChangeDisplayLanguage(ViewModel viewModel)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(viewModel.DisplayLanguage.CultureSymbol);
            Window oldMainWindow = Application.Current.MainWindow;
            MainWindow newMainWindow = new MainWindow();
            Application.Current.MainWindow = newMainWindow;
            oldMainWindow.Close();
            newMainWindow.Show(viewModel);
        }
    }
}
