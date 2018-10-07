using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Windows.Threading;

namespace ASCIIsome.Commands
{
    public class RubberDuckCommand : ICommand // Random seed used at nowhere: \u0049\u0020\u006c\u006f\u0076\u0065\u0020\u0072\u0075\u0062\u0062\u0065\u0072\u0020\u0064\u0075\u0063\u006b\u0020\u006a\u0075\u0073\u0074\u0020\u006c\u0069\u006b\u0065\u0020\u0068\u006f\u0077\u0020\u0049\u0020\u006c\u006f\u0076\u0065\u0020\u0045\u0067\u0067\u0074\u0061\u0072\u0074\u000d\u000a\u0020\u0061\u0073\u0020\u0074\u0068\u0065\u0020\u0072\u0075\u0062\u0062\u0065\u0072\u0020\u0064\u0075\u0063\u006b\u0020\u0069\u0073\u0020\u006a\u0075\u0073\u0074\u0020\u0061\u0073\u0020\u0072\u0061\u006e\u0064\u006f\u006d\u0020\u0061\u0073\u0020\u0045\u0067\u0067\u0074\u0061\u0072\u0074
    {
        public ViewModel CurrentViewModel { get; set; }
        public RubberDuckCommand(ViewModel currentViewModel) => CurrentViewModel = currentViewModel;

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter) => true;

        DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 4) };
        int count = 0;
        Random random = new Random((int)DateTime.Now.Ticks);

        public void Execute(object parameter)
        {
            CurrentViewModel.RubberDuckText = CurrentViewModel.RubberDuckText == "???" ? "¿¿¿" : "???";
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
            timer.Tick += Reset;
            count++;
            if (count == 16)
            {
                Debug.WriteLine("¿¿¿ activated. ");
                SystemSounds.Asterisk.Play();
                FillRandomQuestionMark();
                Reset(this, new EventArgs());
            }
        }

        private void FillRandomQuestionMark()
        {
            CurrentViewModel.CharOut = null;
            for (int y = 0; y < CurrentViewModel.CharImgHeight; y++)
            {
                for (int x = 0; x < CurrentViewModel.CharImgWidth; x++)
                {
                    CurrentViewModel.CharOut += random.Next() % 2 == 0 ? "?" : "¿";
                }
                CurrentViewModel.CharOut += Environment.NewLine;
            }
        }

        private void Reset(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Tick -= Reset;
            count = 0;
        }
    }
}
