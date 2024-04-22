using System;
using System.Windows.Threading;

namespace EpsilonLib.Utils
{
    public class DispatcherEx
    {
        public static IDisposable CreateTimer(TimeSpan interval, Action callback, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return new DisposableTimer(interval, callback, priority);
        }

        public static IDisposable CreateSingleShotTimer(TimeSpan delay, Action callback, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            IDisposable timer = null;
            timer = new DisposableTimer(delay, () =>
            {
                timer.Dispose();
                timer = null;

                callback();
            }, priority);

            return timer;
        }

        class DisposableTimer : IDisposable
        {
            private DispatcherTimer _timer;
            private Action _callback;

            public DisposableTimer(TimeSpan duration, Action callback, DispatcherPriority priority)
            {
                _callback = callback;
                _timer = new DispatcherTimer(priority);
                _timer.Interval = duration;
                _timer.Tick += _timer_Tick;
                _timer.Start();
            }

            private void _timer_Tick(object sender, EventArgs e)
            {
                _callback?.Invoke();
            }

            public void Dispose()
            {
                _callback = null;
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Tick -= _timer_Tick;
                    _timer = null;
                }
            }
        }
    }
}
