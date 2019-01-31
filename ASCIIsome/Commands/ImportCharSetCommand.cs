using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using ASCIIsome.Properties;
using ListBox = System.Windows.Controls.ListBox;

#nullable enable
namespace ASCIIsome.Commands
{
    public sealed class ImportCharSetCommand : CommonCommandBase
    {
        public ImportCharSetCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = Resources.ImportCharSetDialogFilter,
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    File.Copy(filename, Path.Combine(CharSet.CharSetFolderPath, filename.Substring(filename.LastIndexOf('\\') + 1)));
                }
                (parameter as ListBox).SetBinding(ItemsControl.ItemsSourceProperty, (parameter as ListBox).GetBindingExpression(ItemsControl.ItemsSourceProperty).ParentBinding); // [HV] Re-set binding and therefore refresh item list shown in the UI. 
            }
        }
    }
}
