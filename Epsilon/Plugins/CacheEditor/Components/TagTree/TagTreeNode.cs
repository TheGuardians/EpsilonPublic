using EpsilonLib.Shell.TreeModels;

namespace CacheEditor.Components.TagTree
{
    public abstract class TagTreeNode : StandardTreeNode
    {
        public TagTreeNode Parent { get; set; }

        public virtual void UpdateAppearance() { }
    }
}
