using EpsilonLib.Commands;

namespace Epsilon.Commands
{
    [ExportCommand]
    class ShowAboutCommand : CommandDefinition
    {
        public override string Name => "Help.About";

        public override string DisplayText => "About";
    }
}
