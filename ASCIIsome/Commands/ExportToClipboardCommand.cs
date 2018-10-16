using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public class ExportToClipboardCommand : CommonCommandBase
    {
        public ExportToClipboardCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("ExportToClipboard command executed. ");
    }
}
