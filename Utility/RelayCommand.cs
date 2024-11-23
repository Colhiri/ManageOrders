using System;
using System.Windows.Input;

namespace ManageOrders.Utility
{
    /// <summary>
    /// Реализация надстройки для работы с командами
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// Команда
        /// </summary>
        private Action _execute;
        /// <summary>
        /// Проверка возможности выполнения
        /// </summary>
        private Func<bool> _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action execute, Func<bool> canExecute=null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public void Execute(object? parameter) => _execute();
    }
}
