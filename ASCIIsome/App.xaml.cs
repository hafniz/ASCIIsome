using System.Windows;

#nullable enable
namespace ASCIIsome
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU"); // [HV] Override CultureInfo here to display corresponding display language in UI. 
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            CharSet.InitializeCharSetFolder();
        }
    }
}
