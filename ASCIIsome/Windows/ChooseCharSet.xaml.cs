using System.Windows;

#nullable enable
namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for ChooseCharSet.xaml
    /// </summary>
    public partial class ChooseCharSet : Window
    {
        public ChooseCharSet() => InitializeComponent();

        public new void ShowDialog()
        {
            SyncSelectedCharSets();
            base.ShowDialog();
        }

        private void SyncSelectedCharSets()
        {
            foreach (object item in listBox.Items)
            {
                string filename = (((string displayName, string filename))item).filename;
                if ((DataContext as ViewModel).CharSetsInUse.Contains(filename.Substring(filename.LastIndexOf('\\') + 1)))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
            listBox.Focus();
        }
    }
}
