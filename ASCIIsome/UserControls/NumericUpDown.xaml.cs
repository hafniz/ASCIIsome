using System.Windows;
using System.Windows.Controls;

namespace ASCIIsome.UserControls
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public int NumericValue
        {
            get => (int)GetValue(NumericValueProperty);
            set => SetValue(NumericValueProperty, value);
        }
        public static readonly DependencyProperty NumericValueProperty = DependencyProperty.Register("NumericValue", typeof(int), typeof(NumericUpDown), new PropertyMetadata(1));

        public NumericUpDown() => InitializeComponent();

        private void UpwardButton_Click(object sender, RoutedEventArgs e) => NumericValue++;
        private void DownwardButton_Click(object sender, RoutedEventArgs e) => NumericValue--;
    }
}
