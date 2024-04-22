using EpsilonLib.Commands;
using System.Windows.Input;


namespace EpsilonLib.Shell.Commands
{
    [ExportCommand]
    public class CopyCommand : CommandDefinition
    {
        public override string Name => "Edit.Copy";

        public override string DisplayText => "Copy";

        public override KeyShortcut KeyShortcut { get; } = new KeyShortcut(ModifierKeys.Control, Key.C);
    }
}
