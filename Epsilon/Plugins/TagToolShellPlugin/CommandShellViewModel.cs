using EpsilonLib.Commands;
using Stylet;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TagToolShellPlugin
{
    public class CommandShellViewModel : Screen, IDisposable
    {
        private ICommandShell _shell;
        private readonly DelegateCommand _executeCommand;
        private readonly DelegateCommand<string> _navigateHistoryCommand;
        private string _inputText;
        private List<string> _history = new List<string>();
        private int _historyIndex = -1;
        private int _inputTextCaretPosition = 0;

        public event EventHandler<CommandShellOutputEventArgs> OutputReceived;
        public event EventHandler Cleared;

        public CommandShellViewModel(ICommandShell shell)
        {
            _shell = shell;
            _executeCommand = new DelegateCommand(Execute, () => !string.IsNullOrEmpty(InputText));
            _navigateHistoryCommand = new DelegateCommand<string>(NavigateHistory);

            _shell.ContextChanged += _shell_ContextChanged;
            _shell.OutputReceived += Shell_OutputReceived;
        }

        public ICommand ExecuteCommand => _executeCommand;
        public ICommand NavigateHistoryCommand => _navigateHistoryCommand;
        public string ContextName => _shell.ContextDisplayName;

        private void NavigateHistory(string direction)
        {
            if (_history.Count < 1)
                return;

            if (direction == "next")
                _historyIndex--;
            else if (direction == "previous")
                _historyIndex++;

            if (_historyIndex < 0)
                _historyIndex = 0;
            else if (_historyIndex >= _history.Count)
                _historyIndex = _history.Count - 1;

            InputText = _history[_historyIndex];
            InputTextCaretPosition = InputText.Length;
        }

        private void _shell_ContextChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(ContextName));
        }

        public string InputText
        {
            get => _inputText;
            set
            {
                if (SetAndNotify(ref _inputText, value))
                    _executeCommand.RaiseCanExecuteChanged();
            }
        }

        public int InputTextCaretPosition
        {
            get => _inputTextCaretPosition;
            set => SetAndNotify(ref _inputTextCaretPosition, value);
        }

        private async void Execute()
        {
            var inputText = InputText.Trim();
            InputText = string.Empty;

            _history.Remove(inputText);
            _history.Insert(0, inputText);

            _historyIndex = -1;

            if (inputText.Equals("clear", StringComparison.OrdinalIgnoreCase))
            {
                Cleared?.Invoke(this, EventArgs.Empty);
                return;
            }

            OutputReceived?.Invoke(this, new CommandShellOutputEventArgs() { OutputLine = $"{ContextName}> {inputText}" });

            await _shell.ExecuteAsync(inputText);
        }

        private void Shell_OutputReceived(object sender, CommandShellOutputEventArgs e)
        {
            OutputReceived?.Invoke(this, e);
        }
        public void Dispose()
        {
            _shell?.Dispose();
            _shell = null;
        }
    }
}
