using System;

namespace EpsilonLib.Shell
{
    public interface IStatusBar
    {
        string StatusText { get; set; }
        int ProgressPercent { get; set; }
        bool ProgressIndeterminate { get; set; }
        bool ProgressVisible { get; set; }
        void ShowStatusText(string text, TimeSpan? duration = null);

        void ResetStatusText();
    }

    public enum StatusBarProgressState
    {
        None,
        Determinate,
        Indeterminate    
    }
}
