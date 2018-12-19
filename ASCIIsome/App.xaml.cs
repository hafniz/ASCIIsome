using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Windows;

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
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU"); // [HV] Override CultureInfo to display corresponding display language in UI
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            InitializeCharSetFolder();
        }

        private static void InitializeCharSetFolder()
        {
            string charSetFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationInfo.ApplicationName, "CharSets");
            if (!Directory.Exists(charSetFolderPath))
            {
                Directory.CreateDirectory(charSetFolderPath);
                ResourceSet resourceSet = ASCIIsome.Properties.Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                foreach (DictionaryEntry resourceEntry in resourceSet)
                {
                    string resourceName = resourceEntry.Key.ToString();
                    if (resourceName.StartsWith("cs_", StringComparison.InvariantCulture))
                    {
                        using (Stream resourceStream = typeof(App).Assembly.GetManifestResourceStream($"ASCIIsome.Resources.CharSets.{resourceName}.xml"))
                        {
                            using (FileStream fileStream = new FileStream($"{charSetFolderPath}\\{resourceName}.xml", FileMode.Create, FileAccess.Write))
                            {
                                resourceStream.CopyTo(fileStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
