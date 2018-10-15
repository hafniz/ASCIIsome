using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ASCIIsome.Commands;

namespace ASCIIsome
{
    public class ViewModel : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private double windowTop = Application.Current.MainWindow.Top;
        public double WindowTop
        {
            get { return windowTop; }
            set
            {
                windowTop = value;
                OnPropertyChanged(nameof(WindowTop));
            }
        }

        private double windowLeft = Application.Current.MainWindow.Left;
        public double WindowLeft
        {
            get { return windowLeft; }
            set
            {
                windowLeft = value;
                OnPropertyChanged(nameof(WindowLeft));
            }
        }

        private int charImgWidth;
        public int CharImgWidth
        {
            get { return charImgWidth; }
            set
            {
                if (value > 0)
                {
                    charImgWidth = value;
                    OnPropertyChanged(nameof(CharImgWidth));
                    Plotter.Plot(this);
                }
            }
        }

        private int charImgHeight;
        public int CharImgHeight
        {
            get { return charImgHeight; }
            set
            {
                if (value > 0)
                {
                    charImgHeight = value;
                    OnPropertyChanged(nameof(CharImgHeight));
                    Plotter.Plot(this);
                }
            }
        }

        private bool isAspectRatioKept;
        public bool IsAspectRatioKept
        {
            get { return isAspectRatioKept; }
            set
            {
                isAspectRatioKept = value;
                OnPropertyChanged(nameof(IsAspectRatioKept));
                Plotter.Plot(this);
            }
        }

        private bool isDynamicGrayscaleRangeEnabled;
        public bool IsDynamicGrayscaleRangeEnabled
        {
            get { return isDynamicGrayscaleRangeEnabled; }
            set
            {
                isDynamicGrayscaleRangeEnabled = value;
                OnPropertyChanged(nameof(IsDynamicGrayscaleRangeEnabled));
                Plotter.Plot(this);
            }
        }

        private bool isGrayscaleRangeInverted;
        public bool IsGrayscaleRangeInverted
        {
            get { return isGrayscaleRangeInverted; }
            set
            {
                isGrayscaleRangeInverted = value;
                OnPropertyChanged(nameof(IsGrayscaleRangeInverted));
                Plotter.Plot(this);
            }
        }

        private string imgSource;
        public string ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                OnPropertyChanged(nameof(IsGrayscaleRangeInverted));
                Plotter.Plot(this);
            }
        }

        private string charOut;
        public string CharOut
        {
            get { return charOut; }
            set
            {
                charOut = value;
                OnPropertyChanged(nameof(CharOut));
            }
        }

        private string rubberDuckText = "¿¿¿";
        public string RubberDuckText
        {
            get { return rubberDuckText; }
            set
            {
                rubberDuckText = value;
                OnPropertyChanged(nameof(RubberDuckText));
            }
        }

        private CharSetCollection charSetsAvailable;
        public CharSetCollection CharSetsAvailable
        {
            get { return charSetsAvailable; }
            set
            {
                charSetsAvailable = value;
                OnPropertyChanged(nameof(CharSetsAvailable));
            }
        }

        private CharSet currentCharSet;
        public CharSet CurrentCharSet
        {
            get { return currentCharSet; }
            set
            {
                currentCharSet = value;
                OnPropertyChanged(nameof(CurrentCharSet));
                Plotter.Plot(this);
            }
        }

        private DisplayLanguage displayLanguage = DisplayLanguage.GetDisplayLanguageFromSymbol(Thread.CurrentThread.CurrentUICulture.Name);
        public DisplayLanguage DisplayLanguage
        {
            get { return displayLanguage; }
            set
            {
                displayLanguage = value;
                OnPropertyChanged(nameof(DisplayLanguage));
            }
        }

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

        public object Clone() => new ViewModel
        {
            WindowTop = WindowTop,
            WindowLeft = WindowLeft,
            CharImgWidth = CharImgWidth,
            CharImgHeight = CharImgHeight,
            IsAspectRatioKept = IsAspectRatioKept,
            IsDynamicGrayscaleRangeEnabled = IsDynamicGrayscaleRangeEnabled,
            IsGrayscaleRangeInverted = IsGrayscaleRangeInverted,
            ImgSource = ImgSource,
            CharOut = CharOut,
            RubberDuckText = RubberDuckText,
            CharSetsAvailable = CharSetsAvailable,
            CurrentCharSet = CurrentCharSet,
            DisplayLanguage = DisplayLanguage
        };
    }
}
