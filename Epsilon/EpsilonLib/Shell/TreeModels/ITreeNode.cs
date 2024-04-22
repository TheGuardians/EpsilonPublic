using EpsilonLib.Themes;
using System.Collections.Generic;

namespace EpsilonLib.Shell.TreeModels
{
    public interface ITreeNode
    {
        object Tag { get; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        ColorHint TextColor { get; set; }

        IEnumerable<ITreeNode> Children { get; }
    }
}
