using CacheEditor;
using EpsilonLib.Settings;
using Shared;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using TagStructEditor.Common;
using TagStructEditor.Fields;
using TagTool.Cache;

namespace DefinitionEditor
{
    [Export(typeof(ITagEditorPluginProvider))]
    class DefinitionEditorPluginProvider : ITagEditorPluginProvider
    {
        const string ContextKey = "DefinitionEditor.Context";

        private readonly ISettingsCollection _settings;
        private readonly Lazy<IShell> _shell;
       
        [ImportingConstructor]
        public DefinitionEditorPluginProvider(Lazy<IShell> shell, ISettingsService settingsService)
        {
            _shell = shell;
            _settings = settingsService.GetCollection(Settings.CollectionKey);
        }

        public string DisplayName => "Definition";

        public int SortOrder => -1;

        public async Task<ITagEditorPlugin> CreateAsync(TagEditorContext context)
        {
            var valueChangeSink = new ValueChangedSink();

            var config = new TagStructEditor.Configuration()
            {
                OpenTag = context.CacheEditor.OpenTag,
                BrowseTag = context.CacheEditor.RunBrowseTagDialog,
                ValueChanged = valueChangeSink.Invoke,
                DisplayFieldTypes = _settings.Get<bool>(Settings.DisplayFieldTypesSetting),
                DisplayFieldOffsets = _settings.Get<bool>(Settings.DisplayFieldOffsetsSetting),
                CollapseBlocks = _settings.Get<bool>(Settings.CollapseBlocksSetting)
            };

            var ctx = GetDefinitionEditorContext(context);
            var factory = new FieldFactory(ctx.Cache, ctx.TagList, config);

            var definitionData = await context.DefinitionData;
            var field = await Task.Run(() => CreateField(context, factory, definitionData));

            return new DefinitionEditorViewModel(
                _shell.Value,
                context.CacheEditor,
                context.CacheEditor.CacheFile,
                context.Instance,
                definitionData,
                field,
                valueChangeSink,
                config);
        }

        private static StructField CreateField(TagEditorContext context, FieldFactory factory, object definitionData)
        {
            var cache = context.CacheEditor.CacheFile.Cache;
            var structType = cache.TagCache.TagDefinitions.GetTagDefinitionType(context.Instance.Group);

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            var field = factory.CreateStruct(structType);
            Debug.WriteLine($"Create took {stopWatch.ElapsedMilliseconds}ms");

            stopWatch.Restart();

            field.Populate(definitionData);
            Debug.WriteLine($"populate took {stopWatch.ElapsedMilliseconds}ms");

            return field;
        }

        private static PerCacheDefinitionEditorContext GetDefinitionEditorContext(TagEditorContext context)
        {
            var cacheEditor = context.CacheEditor;
            var cache = cacheEditor.CacheFile.Cache;

            if (!cacheEditor.PluginStorage.TryGetValue(ContextKey, out object value) || 
                !ReferenceEquals(cache, (value as PerCacheDefinitionEditorContext).Cache))
            {
                value = new PerCacheDefinitionEditorContext(cache);
                context.CacheEditor.PluginStorage[ContextKey] = value;
            }

            return value as PerCacheDefinitionEditorContext;
        }

        public bool ValidForTag(ICacheFile cache, CachedTag tag)
        {
            return true;
        }

        class PerCacheDefinitionEditorContext
        {
            public GameCache Cache { get; }
            public TagList TagList { get; }

            public PerCacheDefinitionEditorContext(GameCache cache)
            {
                Cache = cache;
                TagList = new TagList(cache);
            }
        }

        class ValueChangedSink : IFieldsValueChangeSink
        {
            public event EventHandler<ValueChangedEventArgs> ValueChanged;

            public void Invoke(ValueChangedEventArgs e)
            {
                ValueChanged?.Invoke(this, e);
            }
        }
    }
}
