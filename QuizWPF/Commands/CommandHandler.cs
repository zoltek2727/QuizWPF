using System;
using System.Windows.Input;

namespace QuizWPF.Commands
{
    class CommandHandler : ICommand
    {
        private Action<object> action;
        private Func<bool> canExecute;
        public CommandHandler(Action<object> action, Func<bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
