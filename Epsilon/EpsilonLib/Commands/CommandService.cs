using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace EpsilonLib.Commands
{
    [Export(typeof(ICommandRegistry))]
    class CommandService : ICommandRegistry
    {
        private readonly Dictionary<Type, Command> _commandByType = new Dictionary<Type, Command>();

        [ImportingConstructor]
        public CommandService(
            [Import] ICommandRouter commandRouter,
            [ImportMany] CommandDefinition[] commands)
        {
            Command.Router = commandRouter;

            foreach(var command in commands)
                RegisterCommand(command);

            //DispatcherEx.CreateTimer(TimeSpan.FromSeconds(1), () =>
            //{
            //    foreach (var command in _commandByType)
            //        command.Value.CanExecute(null);
            //}, 
            //DispatcherPriority.ApplicationIdle);
        }

        private void RegisterCommand(CommandDefinition definition)
        {
            var commandType = definition.GetType();
            _commandByType.Add(commandType, new Command(definition));
        }

        public Command GetCommand(Type commandType)
        {
            return _commandByType.TryGetValue(commandType, out Command command) ? command : null;
        }

        public IEnumerable<Command> GetCommands()
        {
            return _commandByType.Values;
        }
    }
}
