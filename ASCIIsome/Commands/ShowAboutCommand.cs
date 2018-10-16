using ASCIIsome.Windows;
using System.Windows;

namespace ASCIIsome.Commands
{
    public class ShowAboutCommand : CommonCommandBase
    {
        public ShowAboutCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => new About { Owner = Application.Current.MainWindow }.ShowDialog();
    }
}
