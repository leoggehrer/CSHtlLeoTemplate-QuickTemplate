//@BaseCode
//MdStart
using System;
using System.Windows.Input;

namespace QuickTemplate.MvvMApp.ViewModels
{
    public partial class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        private RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Factory mothods
        public static ICommand Create(Action<object?> execute)
        {
            return Create(execute, p => true);
        }
        public static ICommand Create(Action<object?> execute, Predicate<object?>? canExecute)
        {
            return new RelayCommand(execute, canExecute);
        }
        #endregion Factory methods
    }
}
//MdEnd
