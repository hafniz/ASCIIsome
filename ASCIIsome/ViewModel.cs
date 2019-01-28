using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using ASCIIsome.Commands;
using ASCIIsome.Plotting;
using ASCIIsome.Properties;

#nullable enable
namespace ASCIIsome
{
    public class ViewModel : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region properties
        private double mainWindowTop = Application.Current.MainWindow.Top;
        public double MainWindowTop
        {
            get => mainWindowTop;
            set
            {
                mainWindowTop = value;
                OnPropertyChanged(nameof(MainWindowTop));
            }
        }

        private double mainWindowLeft = Application.Current.MainWindow.Left;
        public double MainWindowLeft
        {
            get => mainWindowLeft;
            set
            {
                mainWindowLeft = value;
                OnPropertyChanged(nameof(MainWindowLeft));
            }
        }

        private int charImgWidth = 1;
        public int CharImgWidth
        {
            get => charImgWidth;
            set
            {
                if (value > 0)
                {
                    charImgWidth = value;
                    OnPropertyChanged(nameof(CharImgWidth));
                    Plotter.DebugEnumerateConfig(this); // TODO: [HV] Use ConfigChanged event on actual calling instead
                }
            }
        }

        private int charImgHeight = 1;
        public int CharImgHeight
        {
            get => charImgHeight;
            set
            {
                if (value > 0)
                {
                    charImgHeight = value;
                    OnPropertyChanged(nameof(CharImgHeight));
                    Plotter.DebugEnumerateConfig(this);
                }
            }
        }

        private bool isAspectRatioKept;
        public bool IsAspectRatioKept
        {
            get => isAspectRatioKept;
            set
            {
                isAspectRatioKept = value;
                OnPropertyChanged(nameof(IsAspectRatioKept));
                Plotter.DebugEnumerateConfig(this);
            }
        }

        private bool isDynamicGrayscaleRangeEnabled;
        public bool IsDynamicGrayscaleRangeEnabled
        {
            get => isDynamicGrayscaleRangeEnabled;
            set
            {
                isDynamicGrayscaleRangeEnabled = value;
                OnPropertyChanged(nameof(IsDynamicGrayscaleRangeEnabled));
                Plotter.DebugEnumerateConfig(this);
            }
        }

        private bool isGrayscaleRangeInverted;
        public bool IsGrayscaleRangeInverted
        {
            get => isGrayscaleRangeInverted;
            set
            {
                isGrayscaleRangeInverted = value;
                OnPropertyChanged(nameof(IsGrayscaleRangeInverted));
                Plotter.DebugEnumerateConfig(this);
            }
        }

        private string imgSourcePath;
        public string ImgSourcePath
        {
            get => imgSourcePath;
            set
            {
                imgSourcePath = value;
                OnPropertyChanged(nameof(ImgSourcePath));
                Plotter.DebugEnumerateConfig(this);
            }
        }

        private string charOut;
        public string CharOut
        {
            get => charOut;
            set
            {
                charOut = value;
                OnPropertyChanged(nameof(CharOut));
            }
        }

        private string rubberDuckText = "¿¿¿";
        public string RubberDuckText
        {
            get => rubberDuckText;
            set
            {
                rubberDuckText = value;
                OnPropertyChanged(nameof(RubberDuckText));
            }
        }

        // TODO: [HV] Re-evaluate the necessity of using ObservableCollection
        public static ObservableCollection<(string displayName, string filename)> CharSetsAvailable => new ObservableCollection<(string displayName, string filename)>(CharSet.GetDisplayNames(CharSet.EnumerateFiles()));

        // TODO: [HV] Consider separating this to two properties: List<(string displayName, string filename)> SelectedCharSets and List<CharSet> CharSetsInUse, where the later one can be bound to the listBox
        private List<string> charSetsInUse = new List<string>();
        public List<string> CharSetsInUse
        {
            get => charSetsInUse;
            set
            {
                charSetsInUse = value;
                OnPropertyChanged(nameof(CharSetsInUse));
                Plotter.DebugEnumerateConfig(this);
            }
        }


        private CharSet currentCharSet;
        public CharSet CurrentCharSet
        {
            get => currentCharSet;
            set
            {
                currentCharSet = value;
                OnPropertyChanged(nameof(CurrentCharSet));
                Plotter.DebugEnumerateConfig(this);
            }
        }

        private DisplayLanguage displayLanguage = DisplayLanguage.GetDisplayLanguageFromSymbol(Thread.CurrentThread.CurrentUICulture.Name);
        public DisplayLanguage DisplayLanguage
        {
            get => displayLanguage;
            set
            {
                displayLanguage = value;
                OnPropertyChanged(nameof(DisplayLanguage));
            }
        }

        private string statusBarText;
        public string StatusBarText
        {
            get => statusBarText;
            set
            {
                statusBarText = value;
                OnPropertyChanged(nameof(StatusBarText));
            }
        }
        #endregion

        public object Clone() => new ViewModel
        {
            MainWindowTop = MainWindowTop,
            MainWindowLeft = MainWindowLeft,
            CharImgWidth = CharImgWidth,
            CharImgHeight = CharImgHeight,
            IsAspectRatioKept = IsAspectRatioKept,
            IsDynamicGrayscaleRangeEnabled = IsDynamicGrayscaleRangeEnabled,
            IsGrayscaleRangeInverted = IsGrayscaleRangeInverted,
            ImgSourcePath = ImgSourcePath,
            CharOut = CharOut,
            RubberDuckText = RubberDuckText,
            CharSetsInUse = CharSetsInUse,
            CurrentCharSet = CurrentCharSet,
            DisplayLanguage = DisplayLanguage,
            StatusBarText = StatusBarText
        };

        #region command type declaration
        public ImportFromClipboardCommand ImportFromClipboardCommand { get; set; }
        public OpenFileCommand OpenFileCommand { get; set; }
        public ExportToClipboardCommand ExportToClipboardCommand { get; set; }
        public SaveAsCommand SaveAsCommand { get; set; }
        public ShowChooseCharSetCommand ShowChooseCharSetCommand { get; set; }
        public RubberDuckCommand RubberDuckCommand { get; set; }
        public ShowChangeLanguageCommand ShowChangeLanguageCommand { get; set; }
        public ShowAboutCommand ShowAboutCommand { get; set; }
        public CancelAndCloseCommand CancelAndCloseCommand { get; set; }
        public SubmitLanguageChangeCommand SubmitLanguageChangeCommand { get; set; }
        public SubmitCharSetChoiceCommand SubmitCharSetChoiceCommand { get; set; }
        public ImportCharSetCommand ImportCharSetCommand { get; set; }
        public DeleteCharSetCommand DeleteCharSetCommand { get; set; }
        public OpenCharSetFolderCommand OpenCharSetFolderCommand { get; set; }
        #endregion

        public ViewModel()
        {
            ImportFromClipboardCommand = new ImportFromClipboardCommand(this);
            OpenFileCommand = new OpenFileCommand(this);
            ExportToClipboardCommand = new ExportToClipboardCommand(this);
            SaveAsCommand = new SaveAsCommand(this);
            ShowChooseCharSetCommand = new ShowChooseCharSetCommand(this);
            RubberDuckCommand = new RubberDuckCommand(this);
            ShowChangeLanguageCommand = new ShowChangeLanguageCommand(this);
            ShowAboutCommand = new ShowAboutCommand(this);
            CancelAndCloseCommand = new CancelAndCloseCommand(this);
            SubmitLanguageChangeCommand = new SubmitLanguageChangeCommand(this);
            SubmitCharSetChoiceCommand = new SubmitCharSetChoiceCommand(this);
            ImportCharSetCommand = new ImportCharSetCommand(this);
            DeleteCharSetCommand = new DeleteCharSetCommand(this);
            OpenCharSetFolderCommand = new OpenCharSetFolderCommand(this);
        }

        private static readonly string configFileName = Path.Combine(ApplicationInfo.AppDataFolder, "config.xml");
        private const int latestConfigVersion = 1;
        public void LoadConfig(bool validate = true, int version = latestConfigVersion) // TODO: [HV] Exception handling needed
        {
            // TODO: [HV] Determine the version of config XML automatically by reading xsi:schemaLocation attribute
            if (File.Exists(configFileName))
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                switch (version)
                {
                    case 1:
                        schemaSet.Add(XmlSchema.Read(typeof(ViewModel).Assembly.GetManifestResourceStream("ASCIIsome.Resources.ConfigSchemaV1.xsd"), (sender, e) => throw e.Exception));
                        break;
                    default:
                        break;
                }
                XmlDocument document = new XmlDocument { XmlResolver = new XmlSecureResolver(new XmlUrlResolver(), typeof(ViewModel).Assembly.Evidence), Schemas = schemaSet };
                document.Load(configFileName);
                if (validate)
                {
                    document.Validate((sender, e) => throw e.Exception);
                }
                XmlNode rootNode = document.DocumentElement;
                XmlNodeList nodeList = rootNode.ChildNodes;
                switch (version)
                {
                    case 1:
                        ParseConfigVersion1();
                        break;
                    default:
                        break;
                }
                StatusBarText = Resources.Ready;

                void ParseConfigVersion1()
                {
                    if (nodeList.Cast<XmlNode>().Any(n => n.Name == nameof(DisplayLanguage)) && nodeList.Cast<XmlNode>().Single(n => n.Name == nameof(DisplayLanguage)).InnerText != Thread.CurrentThread.CurrentUICulture.Name)
                    {
                        DisplayLanguage = DisplayLanguage.GetDisplayLanguageFromSymbol(nodeList.Cast<XmlNode>().Single(n => n.Name == nameof(DisplayLanguage)).InnerText);
                        DisplayLanguage.ChangeDisplayLanguage(this, true);
                        return;
                    }
                    foreach (XmlNode xmlNode in nodeList)
                    {
                        switch (xmlNode.Name)
                        {
                            case nameof(IsAspectRatioKept):
                                IsAspectRatioKept = xmlNode.InnerText == "true" ? true : false;
                                break;
                            case nameof(IsDynamicGrayscaleRangeEnabled):
                                IsDynamicGrayscaleRangeEnabled = xmlNode.InnerText == "true" ? true : false;
                                break;
                            case nameof(IsGrayscaleRangeInverted):
                                IsGrayscaleRangeInverted = xmlNode.InnerText == "true" ? true : false;
                                break;
                            case "CharSets":
                                XmlNodeList charSetNodeList = xmlNode.ChildNodes;
                                List<string> fileNames = new List<string>();
                                foreach (XmlNode charSetNode in charSetNodeList)
                                {
                                    fileNames.Add(charSetNode.InnerText);
                                    CharSetsInUse.Add(charSetNode.InnerText);
                                }
                                CurrentCharSet = CharSet.Concat(fileNames);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                StatusBarText = Resources.Welcome;
            }
        }

        public void SaveConfig() // [HV] XML file will be written in the latest version of format only. i.e. This method will be rewritten immediately upon config version is updated
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };
            using (XmlWriter xmlWriter = XmlWriter.Create(configFileName, xmlWriterSettings))
            {
                //xmlWriter.WriteComment(Resources.ConfigFileMessage);
                xmlWriter.WriteStartElement("ASCIIsome.config");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "schemaLocation", null, "ASCIIsome.Resources ConfigSchemaV1.xsd");
                xmlWriter.WriteElementString(nameof(DisplayLanguage), DisplayLanguage.CultureSymbol);
                xmlWriter.WriteElementString(nameof(IsAspectRatioKept), IsAspectRatioKept.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlWriter.WriteElementString(nameof(IsDynamicGrayscaleRangeEnabled), IsDynamicGrayscaleRangeEnabled.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlWriter.WriteElementString(nameof(IsGrayscaleRangeInverted), IsGrayscaleRangeInverted.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlWriter.WriteStartElement("CharSets");
                foreach (string charSetFileName in CharSetsInUse)
                {
                    xmlWriter.WriteElementString("Include", charSetFileName);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
        }
    }
}
