//@BaseCode
//MdStart
using System;
using System.Windows.Input;

namespace QuickTemplate.WpfApp.ViewModels
{
    /// <summary>
    /// Represents a command that executes an action when triggered and can be checked for execution eligibility.
    /// </summary>
    /// <remarks>
    /// This class implements the ICommand interface and provides a simple way to define and handle commands in the MVVM pattern.
    /// </remarks>
    public partial class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Predicate<object?>? canExecute;
        /// <summary>
        /// Initializes a new instance of the RelayCommand class with the specified execute action and optional canExecute predicate.
        /// </summary>
        /// <param name="execute">An action that represents the method to be executed.</param>
        /// <param name="canExecute">A predicate that determines whether the method can be executed. It is optional and can be null.</param>
        private RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }
        
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        
        /// <summary>
        /// Determines whether the specified command can execute with the given parameter.
        /// </summary>
        /// <param name="parameter">The parameter used to determine if the command can execute.</param>
        /// <returns>Returns true if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }
        
        /// <summary>
        /// Executes the method with the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter to be passed to the method.</param>
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
        
        #region Factory mothods
        /// <summary>
        /// Creates a new instance of the <see cref="ICommand"/> interface.
        /// </summary>
        /// <param name="command">A nullable reference to an existing <see cref="ICommand"/> instance. If null, a new instance will be created.</param>
        /// <param name="execute">An <see cref="Action"/> delegate that represents the execution logic of the command.</param>
        /// <returns>A new instance of the <see cref="ICommand"/> interface.</returns>
        public static ICommand Create(ref ICommand? command, Action<object?> execute)
        {
            return Create(ref command, execute, null);
        }
        /// <summary>
        /// Creates a new ICommand instance and assigns it to the provided ICommand reference if it is null.
        /// </summary>
        /// <param name="command">The reference to the ICommand instance to be created or assigned.</param>
        /// <param name="execute">The method to be called when the ICommand is executed.</param>
        /// <param name="canExecute">The method that determines whether the ICommand can be executed.</param>
        /// <returns>The newly created or assigned ICommand instance.</returns>
        public static ICommand Create(ref ICommand? command, Action<object?> execute, Predicate<object?>? canExecute)
        {
            command ??= new RelayCommand(execute, canExecute);
            return command;
        }
        #endregion Factory methods
    }
}
//MdEnd
