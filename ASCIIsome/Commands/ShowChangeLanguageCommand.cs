using ASCIIsome.Windows;
using System.Windows;

namespace ASCIIsome.Commands
{
    public class ShowChangeLanguageCommand : CommonCommandBase
    {
        public ShowChangeLanguageCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new ChangeLanguage { Owner = Application.Current.MainWindow }.ShowDialog((parameter as MainWindow).DataContext as ViewModel);
    }
}
