using System.Windows;
using ASCIIsome.Properties;

#nullable enable
namespace ASCIIsome.Commands
{
    public sealed class ExportToClipboardCommand : CommonCommandBase
    {
        public ExportToClipboardCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            Clipboard.SetText(CurrentViewModel.CharOut);
            CurrentViewModel.StatusBarText = Resources.ExportedToClipboard;
        }
    }
}
