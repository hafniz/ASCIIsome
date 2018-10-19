using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class ImportFromClipboardCommand : CommonCommandBase
    {
        public ImportFromClipboardCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("ImportFromClipboard command executed. ");
    }
}
