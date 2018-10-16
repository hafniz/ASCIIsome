using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public class ManageCharSetCommand : CommonCommandBase
    {
        public ManageCharSetCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("ManageCharSet command executed. ");
    }
}
