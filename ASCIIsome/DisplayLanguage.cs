﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using ASCIIsome.Windows;

namespace ASCIIsome
{
    public sealed class DisplayLanguage
    {
        public string DisplayName { get; }
        public string CultureSymbol { get; }
        public int Index { get; }

        public static DisplayLanguage GetDisplayLanguageFromSymbol(string symbol) => SupportedLanguage.Find(x => x.CultureSymbol == symbol);
        public static DisplayLanguage GetDisplayLanguageFromIndex(int index) => SupportedLanguage.Find(x => x.Index == index);
        public override string ToString() => DisplayName;

        public static List<DisplayLanguage> SupportedLanguage { get; } = new List<DisplayLanguage>
        {
            new DisplayLanguage("English", "en-US", 0),
            new DisplayLanguage("中文（简体）", "zh-CN", 1),
            new DisplayLanguage("русский", "ru-RU", 2)
        };

        private DisplayLanguage(string displayName, string cultureSymbol, int index)
        {
            DisplayName = displayName;
            CultureSymbol = cultureSymbol;
            Index = index;
        }

        public static void ChangeDisplayLanguage(ViewModel viewModel)
        {
            if (viewModel.DisplayLanguage.CultureSymbol != Thread.CurrentThread.CurrentUICulture.Name)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(viewModel.DisplayLanguage.CultureSymbol);
                Window oldMainWindow = Application.Current.MainWindow;
                MainWindow newMainWindow = new MainWindow();
                Application.Current.MainWindow = newMainWindow;
                oldMainWindow?.Close();
                viewModel.StatusBarText = Properties.Resources.LanguageChanged;
                newMainWindow.Show(viewModel);
            }
        }
    }
}
