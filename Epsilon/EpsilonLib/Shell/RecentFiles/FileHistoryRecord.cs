using System;

namespace Shared
{
    public class FileHistoryRecord
    {
        public Guid EditorProviderId { get; }
        public string FilePath { get; }
        public DateTime LastOpened { get; }

        public FileHistoryRecord(Guid editorProviderId, string filePath, DateTime lastOpened)
        {
            EditorProviderId = editorProviderId;
            FilePath = filePath;
            LastOpened = lastOpened;
        }

        public FileHistoryRecord(Guid editorProviderId, string filePath)
        {
            EditorProviderId = editorProviderId;
            FilePath = filePath;
            LastOpened = DateTime.Now;
        }
    }
}
