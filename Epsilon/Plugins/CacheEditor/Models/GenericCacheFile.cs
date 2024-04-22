using System.IO;
using TagTool.Cache;

namespace CacheEditor
{
    public class GenericCacheFile : CacheFileBase
    {
        public GenericCacheFile(FileInfo file, GameCache cache) : base(file, cache)
        {
        }
    }
}
