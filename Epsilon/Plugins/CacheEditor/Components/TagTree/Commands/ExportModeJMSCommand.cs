using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ExportModeJMSCommand : CommandDefinition
    {
        public override string Name => "TagTree.ExportModeJMS";

        public override string DisplayText => "Export mode JMS...";
    }
}
