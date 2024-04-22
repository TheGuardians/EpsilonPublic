using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace CacheEditor
{
    public interface ICacheEditor
    {
        ICacheFile CacheFile { get; }
        ITagTree TagTree { get; }
        ICacheEditorTool GetTool(string name);
        IDictionary<string, object> PluginStorage { get; }
        void OpenTag(CachedTag tag);
        CachedTag RunBrowseTagDialog();
        void Reload();

        CachedTag CurrentTag { get; }
        event EventHandler CurrentTagChanged;
        void ReloadCurrentTag();
        List<string> GetOpenTagNames();
    }
}
