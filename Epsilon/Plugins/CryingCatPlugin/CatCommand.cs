using EpsilonLib.Commands;

namespace CryingCatPlugin
{
    [ExportCommand]
    public class CatCommand : CommandDefinition
    {
        public override string Name => "CryingCats.Test";

        public override string DisplayText => "Cheer up";
    }
}