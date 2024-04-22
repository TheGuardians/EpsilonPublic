using EpsilonLib.Shell;
using System.ComponentModel.Composition;

namespace CacheEditor
{
    class TagExplorerViewModel : CacheEditorTool      
    {
        public const string ToolName = "CacheEditor.TagExplorer";

        public ITagTree TagTree { get; set; }

        public TagExplorerViewModel(ITagTree tagTree)
        {
            Name = ToolName;
            DisplayName = "Tags";
            PreferredLocation = PaneLocation.Left;
            PreferredWidth = 420;    
            TagTree = tagTree;
            IsVisible = true;
            IsActive = true;
        }
        protected override void OnClose()
        {
            base.OnClose();
            TagTree?.Dispose();
            TagTree = null;
        }
    }

    [Export(typeof(ICacheEditorToolProvider))]
    class TagExplorerToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => -1;

        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {
            return new TagExplorerViewModel(editor.TagTree); 
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return true;
        }
    }
}
