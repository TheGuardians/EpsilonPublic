using CacheEditor.Components.TagTree;
using EpsilonLib.Shell.TreeModels;
using Stylet;
using TagTool.Cache;

namespace CacheEditor
{
    class BrowseTagDialogViewModel : Screen
    {
        public TagTreeViewModel TagTree { get; }

        public BrowseTagDialogViewModel(ICacheEditingService cacheEditingService, ICacheFile cacheFile)
        {
            TagTree = new TagTreeViewModel(cacheEditingService, cacheFile);
            TagTree.NodeDoubleClicked += TagTree_NodeDoubleClicked;
            DisplayName = "Tag Browser";
        }

        private void TagTree_NodeDoubleClicked(object sender, TreeNodeEventArgs e)
        {
            if(e.Node.Tag is CachedTag)
                RequestClose(true);
        }
    }
}