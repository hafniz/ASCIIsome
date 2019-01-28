using System.ComponentModel;
using System.Windows;

#nullable enable
namespace ASCIIsome.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        // TODO: [HV] Adjust brightness of the window and controls ('dark mode')

        public MainWindow() : this(true) { }

        public MainWindow(bool isAppStartingUp)
        {
            InitializeComponent();
            if (isAppStartingUp)
            {
                (DataContext as ViewModel).LoadConfig();
            }
            AdjustWindowSize();
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

#warning DebugBreak_Click is for debug use only. 
        private void DebugBreak_Click(object sender, RoutedEventArgs e) { }
    }
}
