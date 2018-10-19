using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class OpenFileCommand : CommonCommandBase
    {
        public OpenFileCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("OpenFile command executed. ");
    }
}
