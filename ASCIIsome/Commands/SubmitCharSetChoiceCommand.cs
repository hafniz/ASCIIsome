using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public sealed class SubmitCharSetChoiceCommand : CommonCommandBase
    {
        public SubmitCharSetChoiceCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("SubmitCharSetChoice command executed. ");
    }
}
