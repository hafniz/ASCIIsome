﻿using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Threading;

namespace ASCIIsome.Commands
{
    public class RubberDuckCommand : CommonCommandBase
    {
        public RubberDuckCommand(ViewModel viewModel) : base(viewModel) { }

        private DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(4) };
        private int count = 0;
        private Random random = new Random((int)(DateTime.Now.Ticks - new DateTime(2002, 9, 18).Ticks));

        public override void Execute(object parameter)
        {
            CurrentViewModel.RubberDuckText = CurrentViewModel.RubberDuckText == "???" ? "¿¿¿" : "???";
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
            timer.Tick -= Reset; // [HV] to prevent subscribing the event repeatedly
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
