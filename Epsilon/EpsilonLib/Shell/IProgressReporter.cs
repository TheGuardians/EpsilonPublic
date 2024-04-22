using System;

namespace Shared
{
    public interface IProgressReporter : IDisposable
    {
        void Report(string statusText, bool indeterminate = true, float completeFraction = 0.0f);
    }
}
