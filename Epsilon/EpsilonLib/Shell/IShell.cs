using EpsilonLib.Shell;
using Stylet;

namespace Shared
{
    public interface IShell
    {
        IObservableCollection<IScreen> Documents { get; }
        IScreen ActiveDocument { get; set; }
        IStatusBar StatusBar { get; }

        IProgressReporter CreateProgressScope();
        bool? ShowDialog(object viewModel);
    }
}
