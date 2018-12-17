using System.Windows;

namespace ASCIIsome.Commands
{
    public sealed class ExportToClipboardCommand : CommonCommandBase
    {
        public ExportToClipboardCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            Clipboard.SetText(CurrentViewModel.CharOut);
            CurrentViewModel.StatusBarText = Properties.Resources.ExportedToClipboard;
        }
    }
}
