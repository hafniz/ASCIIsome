using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCIIsome.Commands;

namespace ASCIIsome
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int charImgWidth = 1;
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

        private int charImgHeight = 1;
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

        private CharSets charSetsAvailable;
        public CharSets CharSetsAvailable
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

        public ImportFromClipboardCommand ImportFromClipboardCommand { get; set; }
        public OpenFileCommand OpenFileCommand { get; set; }
        public ExportToClipboardCommand ExportToClipboardCommand { get; set; }
        public SaveAsCommand SaveAsCommand { get; set; }
        public ManageCharSetCommand ManageCharSetCommand { get; set; }
        public RubberDuckCommand RubberDuckCommand { get; set; }
        public ChangeLanguageCommand ChangeLanguageCommand { get; set; }
        public ShowAboutCommand ShowAboutCommand { get; set; }

        public ViewModel()
        {
            ImportFromClipboardCommand = new ImportFromClipboardCommand(this);
            OpenFileCommand = new OpenFileCommand(this);
            ExportToClipboardCommand = new ExportToClipboardCommand(this);
            SaveAsCommand = new SaveAsCommand(this);
            ManageCharSetCommand = new ManageCharSetCommand(this);
            RubberDuckCommand = new RubberDuckCommand(this);
            ChangeLanguageCommand = new ChangeLanguageCommand(this);
            ShowAboutCommand = new ShowAboutCommand(this);
        }
    }
}
