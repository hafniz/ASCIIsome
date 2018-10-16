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
    public class ShowChangeLanguageCommand : CommonCommandBase
    {
        public ShowChangeLanguageCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new ChangeLanguage { Owner = Application.Current.MainWindow }.ShowDialog((parameter as MainWindow).DataContext as ViewModel);
    }
}
