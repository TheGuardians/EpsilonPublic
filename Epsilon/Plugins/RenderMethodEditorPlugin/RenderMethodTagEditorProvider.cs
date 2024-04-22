using CacheEditor;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace RenderMethodEditorPlugin
{
    [Export(typeof(ITagEditorPluginProvider))]
    class RenderMethodTagEditorProvider : ITagEditorPluginProvider
    {
        public string DisplayName => "Render Method Editor";

        public int SortOrder => 1;

        public async Task<ITagEditorPlugin> CreateAsync(TagEditorContext context)
        {
            RenderMethod rm = null;

            if (context.Instance.IsInGroup("rm  "))
                rm = (RenderMethod)(await context.DefinitionData);
            else if (context.Instance.IsInGroup("prt3"))
                rm = ((Particle)(await context.DefinitionData)).RenderMethod;

            var cache = context.CacheEditor.CacheFile.Cache;
            return new RenderMethodEditorViewModel(cache, rm);
        }

        public bool ValidForTag(ICacheFile cache, CachedTag tag)
        {
            if( cache.Cache is GameCacheGen3 || cache.Cache is GameCacheHaloOnlineBase)
                if (tag.IsInGroup("rm  ") || tag.IsInGroup("prt3"))
                    return true;
            return false;
        }
    }
}
