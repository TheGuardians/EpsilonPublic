using EpsilonLib.Commands;

namespace EpsilonLib.Menus
{
    class CommandMenuItemViewModel : MenuItemViewModel
    {
        private Command _command;
        private bool _isList;

        public CommandMenuItemViewModel(Command command)
        {
            _command = command;
            Text = command.DisplayText;
            IsChecked = command.IsChecked;
            IsEnabled = command.IsEnabled;
            IsVisible = command.IsVisible;
            Command = command;
            command.PropertyChanged += Command_PropertyChanged;
            if(command.Definition != null && command.Definition.KeyShortcut != KeyShortcut.None)
            {
                InputGestureText = command.Definition.KeyShortcut.ToString();
            }

            _isList = typeof(CommandListDefinition).IsAssignableFrom(command.Definition.GetType());
        }

        private void Command_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(Commands.Command.DisplayText):
                case nameof(Commands.Command.IsVisible):
                case nameof(Commands.Command.IsEnabled):
                case nameof(Commands.Command.IsChecked):
                    UpdateVisualState();
                    break;
            }
        }

        protected override void OnAboutToDisplay()
        {
            if(_isList)
            {
                var handler = Commands.Command.Router.GetHandler(_command);
                if (handler != null)
                {
                    Children.Clear();
                    foreach (var command in handler.PopulateCommandList(_command))
                        AddChild(new CommandMenuItemViewModel(command), null);
                }
            }
        }

        private void UpdateVisualState()
        {
            var command = (Command)Command;
            Text = command.DisplayText;
            IsVisible = command.IsVisible;
            IsEnabled = command.IsEnabled;
            IsChecked = command.IsChecked;
        }
    }
}
