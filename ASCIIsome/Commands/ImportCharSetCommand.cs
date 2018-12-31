using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class ImportCharSetCommand : CommonCommandBase
    {
        public ImportCharSetCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("ImportCharSet command executed. ");
    }
}
