using System;
using System.Windows.Input;

namespace WpfApp.ViewModel.Commands
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged = (sender, o) => { };

        private Action<object> _action { get; set; }

        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke(parameter);
        }
    }
}
