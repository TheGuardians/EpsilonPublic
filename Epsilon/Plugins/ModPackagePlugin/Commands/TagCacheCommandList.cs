using EpsilonLib.Commands;

namespace ModPackagePlugin.Commands
{
    [ExportCommand]
    class TagCacheCommandList : CommandListDefinition
    {
        public override string Name => "ModPackage.SelectTagCache";

        public override string DisplayText => "Tag Cache";
    }
}
