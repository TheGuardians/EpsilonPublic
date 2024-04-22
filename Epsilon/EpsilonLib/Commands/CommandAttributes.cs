using System;
using System.ComponentModel.Composition;

namespace EpsilonLib.Commands
{
    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExportCommandHandlerAttribute : ExportAttribute
    {
        public ExportCommandHandlerAttribute() : base(typeof(ICommandTarget))
        {

        }
    }

    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExportCommandAttribute : ExportAttribute
    {
        public ExportCommandAttribute() : base(typeof(CommandDefinition))
        {

        }
    }
}
