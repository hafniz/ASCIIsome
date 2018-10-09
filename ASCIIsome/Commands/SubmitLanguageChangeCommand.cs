using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCIIsome.Commands
{
    public class SubmitLanguageChangeCommand : ICommand
    {
        public ViewModel CurrentViewModel { get; set; }
        public SubmitLanguageChangeCommand(ViewModel currentViewModel) => CurrentViewModel = currentViewModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => DisplayLanguage.ChangeDisplayLanguage(CurrentViewModel); 
    }
}
