using EpsilonLib.Commands;
using System;

namespace Epsilon.Commands
{
    [ExportCommand]
    public class GarbageCollectCommand : CommandDefinition
    {
        public override string Name => "Memory.GarbageCollect";

        public override string DisplayText => "GC";
    }

    [ExportCommandHandler]
    public class GarbageCollectCommandHandler : ICommandHandler<GarbageCollectCommand>
    {
        public void ExecuteCommand(Command command)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void UpdateCommand(Command command)
        {
            
        }
    }
}