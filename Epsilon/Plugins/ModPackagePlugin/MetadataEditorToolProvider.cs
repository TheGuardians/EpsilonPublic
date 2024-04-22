using CacheEditor;
using System.ComponentModel.Composition;
using TagTool.Cache;

namespace ModPackagePlugin
{
    [Export(typeof(ICacheEditorToolProvider))]
    class MetadataEditorToolProvider : ICacheEditorToolProvider
    {
        public int SortOrder => 2;

        public ICacheEditorTool CreateTool(ICacheEditor editor)
        {
            var packageCache = editor.CacheFile.Cache as GameCacheModPackage;

            return new MetadataEditorViewModel(packageCache.BaseModPackage);
        }

        public bool ValidForEditor(ICacheEditor editor)
        {
            return editor.CacheFile.Cache is GameCacheModPackage;
        }
    }
}
