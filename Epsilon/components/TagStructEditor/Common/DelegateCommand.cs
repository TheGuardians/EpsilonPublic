using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TagStructEditor.Common
{
    public class DelegateCommand : ICommand, INotifyPropertyChanged
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute.Invoke();

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }
    }

    public class DelegateCommand<T> : ICommand, INotifyPropertyChanged
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool CanExecute(object parameter) => _canExecute?.Invoke((T)parameter) ?? true;

        public void Execute(object parameter) => _execute.Invoke((T)parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, null);
    }
}
