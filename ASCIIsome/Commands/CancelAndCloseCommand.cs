using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ASCIIsome.Commands
{
    public class CancelAndCloseCommand : ICommand
    {
        public ViewModel CurrentViewModel { get; set; }
        public CancelAndCloseCommand(ViewModel currentViewModel) => CurrentViewModel = currentViewModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => (parameter as Window).Close();
    }
}
