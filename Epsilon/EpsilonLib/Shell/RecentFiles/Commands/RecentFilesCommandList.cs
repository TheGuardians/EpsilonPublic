using EpsilonLib.Commands;

namespace EpsilonLib.Shell.RecentFiles
{
    [ExportCommand]
    class RecentFilesCommandList : CommandListDefinition
    {
        public override string Name => "File.RecentFiles";

        public override string DisplayText => "Recent Files";
    }
}
