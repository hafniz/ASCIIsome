using System.IO;
using System.Windows.Controls;

namespace ASCIIsome.Commands
{
    public sealed class DeleteCharSetCommand : CommonCommandBase
    {
        public DeleteCharSetCommand(ViewModel viewModel) : base(viewModel) { }

        // TODO: [HV] Make this work so that the button is unavailable when no items in the listBox is selected
        //public override bool CanExecute(object parameter) => (parameter as ListBox).SelectedItems.Count != 0; 

        public override void Execute(object parameter)
        {
            foreach (object selectedItem in (parameter as ListBox).SelectedItems)
            {
                File.Delete((((string displayName, string filename))selectedItem).filename);
            }
            (parameter as ListBox).SetBinding(ItemsControl.ItemsSourceProperty, (parameter as ListBox).GetBindingExpression(ItemsControl.ItemsSourceProperty).ParentBinding); // [HV] Reset binding and therefore refresh item list shown in the UI
        }
    }
}
