using System.ComponentModel.Composition;
using TagTool.Cache;

namespace CacheEditor.Components.DependencyExplorer
{
    [Export(typeof(ICacheEditorToolProvider))]
    class DependencyExplorerToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => 1;

        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {
            return new DependencyExplorerViewModel(editor);
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return editor.CacheFile.Cache is GameCacheHaloOnlineBase;
        }
    }
}
