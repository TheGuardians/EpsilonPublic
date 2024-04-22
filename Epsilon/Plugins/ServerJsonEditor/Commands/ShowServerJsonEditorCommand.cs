using EpsilonLib.Commands;

namespace ServerJsonEditor
{
    [ExportCommand]
    class ShowServerJsonEditorCommand : CommandDefinition
    {
        public override string Name => "ServerJsonEditor.Show";

        public override string DisplayText => "Edit Server Voting";
    }
}
