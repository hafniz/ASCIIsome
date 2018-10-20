namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for ChangeLanguage.xaml
    /// </summary>
    public sealed partial class ChangeLanguage
    {
        public ChangeLanguage() => InitializeComponent();

        public void ShowDialog(ViewModel viewModel)
        {
            DataContext = viewModel.Clone();
            ShowDialog();
        }
    }
}
