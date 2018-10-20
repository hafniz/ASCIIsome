namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        public void Show(ViewModel viewModel) // TODO: [HV] Decide startup/minimall height/width of MainWindow in initializing depending on current culture info
        {
            DataContext = viewModel;
            Show();
        }
    }
}
