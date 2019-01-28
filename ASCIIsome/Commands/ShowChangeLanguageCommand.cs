using System.Windows;
using ASCIIsome.Windows;

#nullable enable
namespace ASCIIsome.Commands
{
    public sealed class ShowChangeLanguageCommand : CommonCommandBase
    {
        public ShowChangeLanguageCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new ChangeLanguage { Owner = Application.Current.MainWindow, DataContext = CurrentViewModel.Clone() }.ShowDialog();
    }
}
