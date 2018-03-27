using System;
using System.Windows.Input;

namespace HauntedHunch
{
    /// <summary>
    /// A basic command that runs an action
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action act)
        {
            action = act;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }
    }
}
