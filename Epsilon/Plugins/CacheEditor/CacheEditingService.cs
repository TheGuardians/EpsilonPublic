using EpsilonLib.Settings;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;


namespace CacheEditor
{
    [Export(typeof(ICacheEditingService))]
    class CacheEditingService : ICacheEditingService, IDisposable
    {
        private ICacheEditor _activeEditor;
        private Lazy<IShell> _shell;

        public ISettingsCollection Settings { get; }
        public IReadOnlyList<ITagEditorPluginProvider> TagEditorPlugins { get; }
        public ICacheEditor ActiveCacheEditor 
        {
            get => _activeEditor;
            set => _activeEditor = value;
        }

        public IEnumerable<ICacheEditorToolProvider> Tools { get; set; }

        [ImportingConstructor]
        public CacheEditingService(
            Lazy<IShell> shell,
            ISettingsService settingsService,
            [ImportMany] IEnumerable<ITagEditorPluginProvider> tagEditorPlugins,
            [ImportMany] IEnumerable<ICacheEditorToolProvider> tools)
        {
            _shell = shell;
            Settings = settingsService.GetCollection("CacheEditor");
            TagEditorPlugins = tagEditorPlugins.OrderBy(x => x.SortOrder).ToList();
            Tools = tools;
        }

        public ICacheEditor CreateEditor(ICacheFile cacheFile)
        {
            return new CacheEditorViewModel(_shell.Value, this, cacheFile);
        }

        public void Dispose()
        {
            ActiveCacheEditor = null;
        }
    }
}
