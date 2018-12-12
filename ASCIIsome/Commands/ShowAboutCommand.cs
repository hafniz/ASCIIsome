using System.Windows;
using ASCIIsome.Windows;

namespace ASCIIsome.Commands
{
    public sealed class ShowAboutCommand : CommonCommandBase
    {
        public ShowAboutCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new About { Owner = Application.Current.MainWindow }.ShowDialog();
    }
}
