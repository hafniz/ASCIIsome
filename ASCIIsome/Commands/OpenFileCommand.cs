using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ASCIIsome.Commands
{
    public class OpenFileCommand : CommonCommandBase
    {
        public OpenFileCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            Debug.WriteLine("OpenFile command executed. ");
        }
    }
}
