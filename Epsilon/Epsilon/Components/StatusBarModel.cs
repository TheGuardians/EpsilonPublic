using EpsilonLib.Shell;
using EpsilonLib.Utils;
using Stylet;
using System;

namespace Epsilon.Components
{
    internal class StatusBarModel : PropertyChangedBase, IStatusBar
    {
        const string DefaultText = "Ready";

        private string _statusText = DefaultText;
        private int _progressPercent;
        private bool _progressIndeterminate;
        private bool _progressVisible;
        private IDisposable _clearTimer;

        public string StatusText
        {
            get => _statusText;
            set
            {
                SetAndNotify(ref _statusText, value);
            }
        }

        public int ProgressPercent
        {
            get => _progressPercent;
            set => SetAndNotify(ref _progressPercent, value);
        }

        public bool ProgressIndeterminate
        {
            get => _progressIndeterminate;
            set => SetAndNotify(ref _progressIndeterminate, value);
        }

        public bool ProgressVisible
        {
            get => _progressVisible;
            set => SetAndNotify(ref _progressVisible, value);
        }

        public void ResetStatusText()
        {
            StatusText = DefaultText;
        }

        public void ShowStatusText(string text, TimeSpan? duration = null)
        {
            StatusText = text;

            if (_clearTimer != null)
                _clearTimer.Dispose();

            _clearTimer = DispatcherEx.CreateSingleShotTimer(
                duration.GetValueOrDefault(TimeSpan.FromSeconds(2.5)),
                () =>  {
                    if(StatusText == text)
                        ResetStatusText();
                });
        }
    }

    
}