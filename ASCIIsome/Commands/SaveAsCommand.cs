﻿using System.IO;
using System.Windows.Forms;

namespace ASCIIsome.Commands
{
    public sealed class SaveAsCommand : CommonCommandBase
    {
        public SaveAsCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = Properties.Resources.SaveFileDialogFilter,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, CurrentViewModel.CharOut);
                CurrentViewModel.StatusBarText = Properties.Resources.SavedToFile;
            }
        }
    }
}
