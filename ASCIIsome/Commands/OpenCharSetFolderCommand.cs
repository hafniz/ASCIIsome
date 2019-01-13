using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class OpenCharSetFolderCommand : CommonCommandBase
    {
        public OpenCharSetFolderCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Process.Start("explorer.exe", CharSet.CharSetFolderPath);
    }
}
