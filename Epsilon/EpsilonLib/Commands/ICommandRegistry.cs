using System;
using System.Collections.Generic;

namespace EpsilonLib.Commands
{
    public interface ICommandRegistry
    {
        IEnumerable<Command> GetCommands();
        Command GetCommand(Type commandType);
    }
}
