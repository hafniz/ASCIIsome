using System.Windows.Forms;
using ASCIIsome.Properties;

namespace ASCIIsome.Commands
{
    public sealed class OpenFileCommand : CommonCommandBase
    {
        public OpenFileCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = Resources.OpenFileDialogFilter, // TODO: [HV] Add all files filter once TextImageConverter call is implemented
                FilterIndex = 8,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CurrentViewModel.ImgSourcePath = openFileDialog.FileName; // TODO: [HV] Optimize scaling algorithm of Image control in MainWindow for image display
                CurrentViewModel.StatusBarText = Resources.ImageLoaded;
            }
        }
    }
}
