using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Threading;

namespace ASCIIsome.Commands
{
    public sealed class RubberDuckCommand : CommonCommandBase
    {
        public RubberDuckCommand(ViewModel viewModel) : base(viewModel) { }

        private static readonly DateTime dateTimeOffset = new DateTime(2002, 9, 18);
        private readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
        private readonly Random random = new Random((int)(DateTime.Now.Ticks - dateTimeOffset.Ticks));
        private int count = 0;

        public override void Execute(object parameter)
        {
            CurrentViewModel.RubberDuckText = CurrentViewModel.RubberDuckText == "???" ? "¿¿¿" : "???";
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
            timer.Tick -= Reset; // [HV] To prevent subscribing the event repeatedly
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
