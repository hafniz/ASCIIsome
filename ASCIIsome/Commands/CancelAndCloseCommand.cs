using System.Windows;

namespace ASCIIsome.Commands
{
    public sealed class CancelAndCloseCommand : CommonCommandBase
    {
        public CancelAndCloseCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => (parameter as Window).Close();
    }
}
