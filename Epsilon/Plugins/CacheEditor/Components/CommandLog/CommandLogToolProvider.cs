using EpsilonLib.Logging;
using System.ComponentModel.Composition;
using TagTool.Cache;

namespace CacheEditor.Components.CommandLog
{
    [Export(typeof(ICacheEditorToolProvider))]
    class CommandLogToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => 1;
        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {          
            return new CommandLogViewModel(editor);
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return editor.CacheFile.Cache is GameCacheHaloOnlineBase;
        }
    }
}
