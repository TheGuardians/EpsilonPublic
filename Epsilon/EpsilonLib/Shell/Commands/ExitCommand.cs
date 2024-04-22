using EpsilonLib.Commands;
using System.Windows.Input;

namespace EpsilonLib.Shell.Commands
{
    [ExportCommand]
    public class ExitCommand : CommandDefinition
    {
        public override string Name => "Application.Exit";

        public override string DisplayText => "Exit";

        public override KeyShortcut KeyShortcut { get; } = new KeyShortcut(ModifierKeys.Alt, Key.F4);
    }
}
