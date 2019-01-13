using System.Windows;

namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for ChooseCharSet.xaml
    /// </summary>
    public partial class ChooseCharSet : Window
    {
        public ChooseCharSet() => InitializeComponent();

        public void ShowDialog(ViewModel viewModel)
        {
            DataContext = viewModel.Clone();
            foreach (object item in listBox.Items)
            {
                string filename = (((string displayName, string filename))item).filename;
                if (viewModel.CharSetsInUse.Contains(filename.Substring(filename.LastIndexOf('\\') + 1)))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
            listBox.Focus();
            ShowDialog();
        }
    }
}
