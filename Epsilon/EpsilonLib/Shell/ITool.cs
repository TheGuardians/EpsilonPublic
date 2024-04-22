using EpsilonLib.Shell;
using Stylet;

namespace Shared
{
    public interface IToolFactory
    {
        ITool CreateTool();
    }

    public interface ITool : IScreen
    {
        string Name { get; }
        PaneLocation PreferredLocation { get; }
        int PreferredWidth { get; }
        int PreferredHeight { get; }
        bool IsVisible { get; set; }
        new bool IsActive { get; set; }
    }
}
