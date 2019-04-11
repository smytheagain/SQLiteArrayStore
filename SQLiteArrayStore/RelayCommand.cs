using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SQLiteArrayStore
{
  /// <summary>
  /// A command whose sole purpose is to
  /// relay its functionality to other
  /// objects by invoking delegates. The
  /// default return value for the CanExecute
  /// method is 'true'.
  /// </summary>
  public class RelayCommand : ICommand
  {
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    /// <summary>
    /// Creates a new command that can always execute.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
      _execute = execute ?? throw new ArgumentNullException($"{nameof(execute)}");
      _canExecute = canExecute;
    }

    /// <summary>
    /// Returns whether or not the command can be executed.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    /// <returns>True when possible to execute, else returns false.</returns>
    [DebuggerStepThrough]
    public bool CanExecute(object parameters)
    {
      return _canExecute == null ? true : _canExecute(parameters);
    }

    /// <summary>
    /// Raised when the Execute property values has changed.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    /// <summary>
    /// Execute the command.
    /// </summary>
    /// <param name="parameters">The command parameters.</param>
    public void Execute(object parameters)
    {
      _execute(parameters);
    }
  }
}
