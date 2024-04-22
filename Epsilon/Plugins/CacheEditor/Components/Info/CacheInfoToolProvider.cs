using System.ComponentModel.Composition;

namespace CacheEditor.Components.Info
{
    [Export(typeof(ICacheEditorToolProvider))]
    class CacheInfoToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => -1;

        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {
            return new CacheInfoToolViewModel(editor);
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return true;
        }
    }
}
