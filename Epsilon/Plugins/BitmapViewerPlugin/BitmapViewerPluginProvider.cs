using CacheEditor;
using Shared;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace BitmapViewerPlugin
{
    [Export(typeof(ITagEditorPluginProvider))]
    class BitmapViewerPluginProvider : ITagEditorPluginProvider
    {
        private readonly Lazy<IShell> _shell;

        [ImportingConstructor]
        public BitmapViewerPluginProvider(Lazy<IShell> shell)
        {
            _shell = shell;
        }

        public string DisplayName => "Bitmap";

        public int SortOrder => 6;

        public async Task<ITagEditorPlugin> CreateAsync(TagEditorContext context)
        {
            if(CacheVersionDetection.IsInGen(CacheGeneration.Second, context.CacheEditor.CacheFile.Cache.Version))
            {
                var definition = await context.DefinitionData as TagTool.Tags.Definitions.Gen2.Bitmap;
                return new BitmapViewerViewModel(context.CacheEditor.CacheFile, context.Instance, definition);
            }
            else
            {
                var definition = await context.DefinitionData as Bitmap;
                return new BitmapViewerViewModel(context.CacheEditor.CacheFile, context.Instance, definition);
            }
        }

        public bool ValidForTag(ICacheFile cache, CachedTag tag)
        {
            return tag.IsInGroup("bitm");
        }
    }
}
