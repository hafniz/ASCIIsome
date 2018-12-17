namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            (DataContext as ViewModel).StatusBarText = Properties.Resources.Ready; // TODO: [HV] Show random tips on status bar on startup
        }

        public void Show(ViewModel viewModel) // TODO: [HV] Decide startup/minimal height/width of MainWindow in initializing depending on current culture info
        {
            DataContext = viewModel;
            Show();
        }
    }
}
