using System.Windows;
using ASCIIsome.Windows;

namespace ASCIIsome.Commands
{
    public sealed class ShowChooseCharSetCommand : CommonCommandBase
    {
        public ShowChooseCharSetCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new ChooseCharSet { Owner = Application.Current.MainWindow }.ShowDialog(CurrentViewModel);
    }
}
