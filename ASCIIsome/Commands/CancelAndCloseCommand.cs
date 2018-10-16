using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ASCIIsome.Commands
{
    public class CancelAndCloseCommand : CommonCommandBase
    {
        public CancelAndCloseCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => (parameter as Window).Close();
    }
}
