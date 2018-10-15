using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ASCIIsome.Windows;

namespace ASCIIsome.Commands
{
    public class ShowChangeLanguageCommand : ICommand
    {
        public ViewModel CurrentViewModel { get; set; }
        public ShowChangeLanguageCommand(ViewModel currentViewModel) => CurrentViewModel = currentViewModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => new ChangeLanguage { Owner = Application.Current.MainWindow }.ShowDialog((parameter as MainWindow).DataContext as ViewModel);
    }
}
