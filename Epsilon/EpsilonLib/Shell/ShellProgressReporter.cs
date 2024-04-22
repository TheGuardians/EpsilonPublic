using EpsilonLib.Shell;
using System.Windows.Threading;

namespace Shared
{
    public sealed class ShellProgressReporter : IProgressReporter
    {
        private readonly IStatusBar _statusBar;
        private readonly Dispatcher _dispatcher;

        public ShellProgressReporter(IStatusBar statusBar)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _statusBar = statusBar;
        }

        public void Dispose()
        {
            _dispatcher.Invoke(() =>
            {
                _statusBar.ResetStatusText();
                _statusBar.ProgressVisible = false;
            });
        }

        void IProgressReporter.Report(string statusText, bool indeterminate, float completeFraction)
        {
            _dispatcher.Invoke(() =>
            {
                _statusBar.StatusText = statusText;
                _statusBar.ProgressVisible = (1 - completeFraction) > 0.001f;
                _statusBar.ProgressIndeterminate = indeterminate;
                _statusBar.ProgressPercent = (int)(completeFraction * 100);
            });
        }
    }
}
