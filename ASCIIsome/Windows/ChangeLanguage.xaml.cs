using System.Windows;

namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for ChangeLanguage.xaml
    /// </summary>
    public partial class ChangeLanguage : Window
    {
        public ChangeLanguage() => InitializeComponent();

        public void ShowDialog(ViewModel viewModel)
        {
            DataContext = viewModel.Clone();
            ShowDialog();
        }
    }
}
