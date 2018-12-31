using System.ComponentModel;
using System.Windows;

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
            AdjustWindowSize();
            (DataContext as ViewModel).LoadConfig();
        }

        public void Show(ViewModel viewModel) 
        {
            DataContext = viewModel;
            AdjustWindowSize();
            Show();
        }

        private void AdjustWindowSize()
        {
            // TODO: [HV] Decide startup/minimal height/width of MainWindow in initializing depending on current culture info
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Application.Current.MainWindow == this)
            {
                (DataContext as ViewModel).SaveConfig();
            }
            base.OnClosing(e);
        }
    }
}
