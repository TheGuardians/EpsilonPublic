using System.Threading.Tasks;
using TagTool.Cache;

namespace CacheEditor
{
    public interface ITagEditorPluginProvider
    {
        string DisplayName { get; }

        int SortOrder { get; }

        Task<ITagEditorPlugin> CreateAsync(TagEditorContext context);

        bool ValidForTag(ICacheFile cache, CachedTag tag);
    }
}
