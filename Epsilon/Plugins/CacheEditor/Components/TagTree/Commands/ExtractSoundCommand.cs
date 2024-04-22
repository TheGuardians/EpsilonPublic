using EpsilonLib.Commands;

namespace CacheEditor.Components.TagTree.Commands
{
    [ExportCommand]
    public class ExtractSoundCommand : CommandDefinition
    {
        public override string Name => "TagTree.ExtractSound";

        public override string DisplayText => "Extract Sound...";
    }
}
