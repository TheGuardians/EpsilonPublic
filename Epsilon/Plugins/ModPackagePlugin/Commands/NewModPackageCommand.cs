using EpsilonLib.Commands;
using System.Windows.Input;

namespace ModPackagePlugin.Commands
{
    [ExportCommand]
    class NewModPackageCommand : CommandDefinition
    {
        public override string Name => "ModPackage.New";

        public override string DisplayText => "Mod Package...";

        public override KeyShortcut KeyShortcut => new KeyShortcut(ModifierKeys.Control, Key.N);

    }
}
