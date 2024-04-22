using EpsilonLib.Utils;
using System;
using System.Windows;
using System.Windows.Threading;

namespace EpsilonLib.Shell
{
    public static class ClipboardEx
    {
        private const int MaxRetries = 10;
        private const int RetryDelayMs = 100;
        private static IDisposable _timer;
        private static object _mutex = new object();

        public static void SetTextSafe(string text, int retryCount = MaxRetries)
        {
            void StopTimer()
            {
                _timer?.Dispose();
                _timer = null;
            }

            lock (_mutex)
            {
                StopTimer();
                _timer = DispatcherEx.CreateTimer(TimeSpan.FromMilliseconds(RetryDelayMs), () =>
                {
                    try
                    {
                        Clipboard.SetText(text);
                        StopTimer();
                    }
                    catch
                    {
                        if (--retryCount == 0)
                        {
                            StopTimer();
                            throw;
                        }
                    }
                }, DispatcherPriority.Input);
            }
        }
    }
}
