using EpsilonLib.Commands;

namespace CacheEditor.Commands
{
    [ExportCommand]
    class ShowInfoWindowCommand : CommandDefinition
    {
        public override string Name => "CacheEditor.ShowInfoWindow";

        public override string DisplayText => "Info";
    }
}
