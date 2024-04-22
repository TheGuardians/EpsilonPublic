using System.IO;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;

namespace CacheEditor
{
    public class HaloOnlineCacheFile : CacheFileBase
    {
        private new GameCacheHaloOnlineBase Cache => (GameCacheHaloOnlineBase) base.Cache;
 
        public HaloOnlineCacheFile(FileInfo file, GameCache cache) : base(file, cache)
        {
        }

        public override bool CanDeleteTag => true;
        public override bool CanExtractTag => true;
        public override bool CanRenameTag => true;
        public override bool CanDuplicateTag => true;
        public override bool CanSerializeTags => true;
        public override bool CanImportTag => true;

        public override void DeleteTag(CachedTag tag)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.TagCacheGenHO.Tags[tag.Index] = null;
                Cache.TagCacheGenHO.SetTagDataRaw(stream, (CachedTagHaloOnline)tag, new byte[] { });
                Cache.SaveTagNames();
            }
        }

        public override void ExtractTag(CachedTag tag, string filePath)
        {
            using (var stream = Cache.OpenCacheRead())
            {
                var data = Cache.TagCacheGenHO.ExtractTagRaw(stream, tag as CachedTagHaloOnline);
                using (var outStream = System.IO.File.Create(filePath))
                    outStream.Write(data, 0, data.Length);
            }
        }

        public override void ImportTag(CachedTag tag, string filePath)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                var data = System.IO.File.ReadAllBytes(filePath);
                Cache.TagCacheGenHO.SetTagDataRaw(stream, tag as CachedTagHaloOnline, data);
            }
        }

        public override void DuplicateTag(CachedTag tag, string newName)
        {
            var newTag = Cache.TagCache.AllocateTag(tag.Group, newName);
            Cache.SaveTagNames();
            using (var stream = Cache.OpenCacheReadWrite())
            {
                var originalDefinition = Cache.Deserialize(stream, tag);
                Cache.Serialize(stream, newTag, originalDefinition);
            }
        }

        public async override Task DoSerializeTagAsync(CachedTag instance, object definition)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                await Task.Run(() => 
                {
                    // File -> Save when
                    Cache.Serialize(stream, instance, definition);
                    Cache.SaveStrings();
                    Cache.SaveTagNames();
                });
            }
               
        }

        public override void RenameTag(CachedTag tag, string newName)
        {
            tag.Name = newName;
            Cache.SaveTagNames();
        }
    }
}
