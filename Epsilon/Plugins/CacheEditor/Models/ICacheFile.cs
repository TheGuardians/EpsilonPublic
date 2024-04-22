using System;
using System.IO;
using System.Threading.Tasks;
using TagTool.Cache;

namespace CacheEditor
{
    public interface ICacheFile
    {
        event EventHandler Reloaded;
        event EventHandler<CachedTag> TagSerialized;

        FileInfo File { get; }
        GameCache Cache { get; set; }

        bool CanExtractTag { get; }
        bool CanRenameTag { get; }
        bool CanDeleteTag { get; }
        bool CanDuplicateTag { get; }
        bool CanSerializeTags { get; }
        bool CanImportTag { get; }

        void ExtractTag(CachedTag tag, string filePath);
        void ImportTag(CachedTag tag, string filePath);
        void RenameTag(CachedTag tag, string newName);
        void DeleteTag(CachedTag tag);
        void DuplicateTag(CachedTag tag, string newName);
        Task SerializeTagAsync(CachedTag instance, object definition);
    }
}
