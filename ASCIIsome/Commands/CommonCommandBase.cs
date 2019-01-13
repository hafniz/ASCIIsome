using System;
using System.Windows.Input;

namespace ASCIIsome.Commands
{
    public abstract class CommonCommandBase : ICommand
    {
        protected ViewModel CurrentViewModel { get; }
        protected CommonCommandBase(ViewModel viewModel) => CurrentViewModel = viewModel;

        public event EventHandler CanExecuteChanged;
        protected void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());
        public virtual bool CanExecute(object parameter) => true;
        public abstract void Execute(object parameter);
    }
}
