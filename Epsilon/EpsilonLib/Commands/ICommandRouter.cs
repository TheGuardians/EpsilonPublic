using System;
using System.Collections.Generic;

namespace EpsilonLib.Commands
{
    public interface ICommandRouter
    {
        ICommandHandler GetHandler(Command command);
    }

    public interface ICommandTarget
    {

    }

    public interface ICommandHandler
    {
        bool CanUpdate { get; }
        bool CanPopulate { get; }

        ICommandTarget Target { get; }

        void ExecuteCommand(Command command);
        void UpdateCommand(Command command);
        IEnumerable<Command> PopulateCommandList(Command command);
    }

    public class CommandHandler<T> : ICommandHandler
    {
        public ICommandTarget Target { get; }

        public CommandHandler(ICommandTarget target)
        {
            Target = target;
        }

        public bool CanUpdate => Target is ICommandHandler<T>;
        public bool CanPopulate=> Target is ICommandListPopulator<T>;

        public void ExecuteCommand(Command command)
        {
            if (Target is ICommandHandler<T> handler)
                handler.ExecuteCommand(command);
        }

        public void UpdateCommand(Command command)
        {
            if(Target is ICommandHandler<T> handler)
                handler.UpdateCommand(command);
        }

        public IEnumerable<Command> PopulateCommandList(Command command)
        {
            if (Target is ICommandListPopulator<T> handler)
                return handler.PopulateCommandList(command);

            throw new NotSupportedException();
        }
    }

    public interface ICommandHandler<T> : ICommandTarget
    {
        void ExecuteCommand(Command command);
        void UpdateCommand(Command command);
    }

    public interface ICommandListPopulator<T> : ICommandTarget
    {
        IEnumerable<Command> PopulateCommandList(Command command);
    }
}
