using System;
using System.Windows.Input;

namespace HauntedHunch
{
    /// <summary>
    /// A command that can execute only once
    /// </summary>
    public class ActivatableOnlyOnceCommand : ICommand
    {
        private Action mAction;

        private bool activated;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public ActivatableOnlyOnceCommand(Action action)
        {
            mAction = action;
            activated = false;
        }

        public bool CanExecute(object parameter) => activated;

        public void Execute(object parameter)
        {
            mAction();
            activated = true;
        }
    }
}
