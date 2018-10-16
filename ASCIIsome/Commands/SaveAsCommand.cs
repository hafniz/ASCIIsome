﻿using System.Diagnostics;

namespace ASCIIsome.Commands
{
    public class SaveAsCommand : CommonCommandBase
    {
        public SaveAsCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter) => Debug.WriteLine("SaveAs command executed. ");
    }
}
