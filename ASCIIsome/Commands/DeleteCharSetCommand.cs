using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class DeleteCharSetCommand : CommonCommandBase
    {
        public DeleteCharSetCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("DeleteCharSet command executed. ");
    }
}
