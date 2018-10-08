﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ASCIIsome
{
    /// <summary>
    /// Interaction logic for ChangeLanguage.xaml
    /// </summary>
    public partial class ChangeLanguage : Window
    {
        public ChangeLanguage()
        {
            InitializeComponent();
        }

        public void Show(ViewModel viewModel)
        {
            Resources["viewModel"] = viewModel;
            Show();
        }
    }
}