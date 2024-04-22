using EpsilonLib.Settings;
using System;
using System.Collections.Generic;

namespace CacheEditor
{
    public interface ICacheEditingService : IDisposable
    {
        ICacheEditor ActiveCacheEditor { get; set; }
        ISettingsCollection Settings { get; }
        IReadOnlyList<ITagEditorPluginProvider> TagEditorPlugins { get; }
        IEnumerable<ICacheEditorToolProvider> Tools { get; }
        ICacheEditor CreateEditor(ICacheFile cacheFile);
    }
}
