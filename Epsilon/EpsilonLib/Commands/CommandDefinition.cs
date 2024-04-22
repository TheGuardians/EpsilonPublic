using System.Globalization;
using System.Windows.Input;

namespace EpsilonLib.Commands
{
    public abstract class CommandDefinition
    {
        public virtual bool VisibleByDefault { get; } = true; 
        public abstract string Name { get; }
        public abstract string DisplayText { get; }
        public virtual KeyShortcut KeyShortcut { get; } = KeyShortcut.None;
    }

    public class KeyShortcut
    {
        public KeyGesture KeyGesture { get; }

        private KeyShortcut()
        {

        }

        public KeyShortcut(ModifierKeys modifierKeys, Key key)
        {
            KeyGesture = new KeyGesture(key, modifierKeys);
        }

        public static readonly KeyShortcut None = new KeyShortcut();

        public override string ToString()
        {
            if (KeyGesture == null)
                return "None";

            return KeyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture);
        }
    }
}