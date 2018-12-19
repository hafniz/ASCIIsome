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
            ShowDialog();
        }
    }
}
