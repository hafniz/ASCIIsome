using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using ASCIIsome.Commands;
using ASCIIsome.Plotting;

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
                    Plotter.OutputEnumerateConfig(this); // TODO: [HV] Use ConfigChanged event on actual calling instead
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
                    Plotter.OutputEnumerateConfig(this);
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
                Plotter.OutputEnumerateConfig(this);
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
                Plotter.OutputEnumerateConfig(this);
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
                Plotter.OutputEnumerateConfig(this);
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
                Plotter.OutputEnumerateConfig(this);
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

        private ObservableCollection<CharSet> charSetsAvailable;
        public ObservableCollection<CharSet> CharSetsAvailable
        {
            get => charSetsAvailable;
            set
            {
                charSetsAvailable = value;
                OnPropertyChanged(nameof(CharSetsAvailable));
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
                Plotter.OutputEnumerateConfig(this);
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
            CharSetsAvailable = CharSetsAvailable,
            CurrentCharSet = CurrentCharSet,
            DisplayLanguage = DisplayLanguage
        };

        #region command type declaration
        public ImportFromClipboardCommand ImportFromClipboardCommand { get; set; }
        public OpenFileCommand OpenFileCommand { get; set; }
        public ExportToClipboardCommand ExportToClipboardCommand { get; set; }
        public SaveAsCommand SaveAsCommand { get; set; }
        public ManageCharSetCommand ManageCharSetCommand { get; set; }
        public RubberDuckCommand RubberDuckCommand { get; set; }
        public ShowChangeLanguageCommand ShowChangeLanguageCommand { get; set; }
        public ShowAboutCommand ShowAboutCommand { get; set; }
        public CancelAndCloseCommand CancelAndCloseCommand { get; set; }
        public SubmitLanguageChangeCommand SubmitLanguageChangeCommand { get; set; }
        #endregion

        public ViewModel()
        {
            ImportFromClipboardCommand = new ImportFromClipboardCommand(this);
            OpenFileCommand = new OpenFileCommand(this);
            ExportToClipboardCommand = new ExportToClipboardCommand(this);
            SaveAsCommand = new SaveAsCommand(this);
            ManageCharSetCommand = new ManageCharSetCommand(this);
            RubberDuckCommand = new RubberDuckCommand(this);
            ShowChangeLanguageCommand = new ShowChangeLanguageCommand(this);
            ShowAboutCommand = new ShowAboutCommand(this);
            CancelAndCloseCommand = new CancelAndCloseCommand(this);
            SubmitLanguageChangeCommand = new SubmitLanguageChangeCommand(this);
        }
    }
}
