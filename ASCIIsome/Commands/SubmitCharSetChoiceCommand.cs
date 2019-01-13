using System.Collections.Generic;
using System.Windows;
using ASCIIsome.Windows;

namespace ASCIIsome.Commands
{
    public sealed class SubmitCharSetChoiceCommand : CommonCommandBase
    {
        public SubmitCharSetChoiceCommand(ViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            List<string> charSetsInUse = new List<string>();
            foreach (object selectedItem in (parameter as ChooseCharSet).listBox.SelectedItems)
            {
                string fullPath = (((string displayName, string filename))selectedItem).filename;
                charSetsInUse.Add(fullPath.Substring(fullPath.LastIndexOf('\\') + 1));
            }
            (Application.Current.MainWindow.DataContext as ViewModel).CharSetsInUse = charSetsInUse; // TODO: [HV] Validation still needed
            (Application.Current.MainWindow.DataContext as ViewModel).CurrentCharSet = CharSet.Concat(charSetsInUse); // TODO: [HV] Store a cache of the concatenated CharSet and looking for cache before calling Concat() to save time
            (parameter as Window)?.Close();
        }
    }
}
