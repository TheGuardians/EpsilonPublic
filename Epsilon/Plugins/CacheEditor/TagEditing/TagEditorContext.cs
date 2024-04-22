using System.Threading.Tasks;
using TagTool.Cache;

namespace CacheEditor
{
    public class TagEditorContext
    {
        public ICacheEditor CacheEditor { get; set; }
        public CachedTag Instance { get; set; }
        public Task<object> DefinitionData { get; set; }
    }
}
