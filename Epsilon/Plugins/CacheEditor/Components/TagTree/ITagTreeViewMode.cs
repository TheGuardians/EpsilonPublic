using EpsilonLib.Shell.TreeModels;
using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace CacheEditor.Components.TagTree
{
    interface ITagTreeViewMode
    {
        IEnumerable<ITreeNode> BuildTree(GameCache cache, Func<CachedTag, bool> filter);
    }
}
