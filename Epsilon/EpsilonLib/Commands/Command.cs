using Stylet;
using System;
using System.Windows.Input;

namespace EpsilonLib.Commands
{
    public class Command : PropertyChangedBase, ICommand
    {
        internal static ICommandRouter Router { get; set; }

        private string _displayText;
        private bool _isVisible = true;
        private bool _isEnabled = true;
        private bool _isChecked = false;

        public Command()
        {

        }

        public Command(CommandDefinition definition)
        {
            Definition = definition;
            DisplayText = definition.DisplayText;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandDefinition Definition { get; set; }
        public object Tag { get; set; }
        public bool RequiresUpdate { get; set; } = true;

        public string DisplayText
        {
            get => _displayText;
            set => SetAndNotify(ref _displayText, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetAndNotify(ref _isEnabled, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetAndNotify(ref _isVisible, value);
        }

        public bool CanExecute(object parameter)
        {
            if (!RequiresUpdate)
                return true;

            var handler = Router.GetHandler(this);
            if (handler == null)
                return false;

            handler.UpdateCommand(this);
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            var handler = Router.GetHandler(this);
            if (handler != null)
                handler.ExecuteCommand(this);
        }
    }
}
