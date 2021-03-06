﻿using System.Windows;
using ASCIIsome.Windows;

#nullable enable
namespace ASCIIsome.Commands
{
    public sealed class SubmitLanguageChangeCommand : CommonCommandBase
    {
        public SubmitLanguageChangeCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            ((Window)parameter).DialogResult = true;
            DisplayLanguage.ChangeDisplayLanguage((parameter as ChangeLanguage)?.DataContext as ViewModel);
        }
    }
}
