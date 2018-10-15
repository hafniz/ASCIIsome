using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ASCIIsome.Windows;

namespace ASCIIsome.Commands
{
    public class SubmitLanguageChangeCommand : ICommand
    {
        public ViewModel CurrentViewModel { get; set; }
        public SubmitLanguageChangeCommand(ViewModel currentViewModel) => CurrentViewModel = currentViewModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            (parameter as Window).DialogResult = true;
            DisplayLanguage.ChangeDisplayLanguage((parameter as ChangeLanguage).DataContext as ViewModel);
        }
    }
}
