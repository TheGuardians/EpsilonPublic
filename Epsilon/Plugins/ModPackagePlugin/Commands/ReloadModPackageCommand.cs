using EpsilonLib.Commands;
using System.Windows.Input;

namespace ModPackagePlugin.Commands
{
    [ExportCommand]
    class ReloadModPackageCommand : CommandDefinition
    {
        public override string Name => "ModPackage.Reload";

        public override string DisplayText => "Reload";

        public override KeyShortcut KeyShortcut => new KeyShortcut(ModifierKeys.Control, Key.R);
    }
}
