using EpsilonLib.Shell.TreeModels;
using System.Runtime.CompilerServices;
using System;

namespace CacheEditor
{
    public interface ITagTree : IDisposable
    {
        ITreeNode SelectedNode { get; }

        void UpdateNodeAppearance(ITagTree node);

    }
}
