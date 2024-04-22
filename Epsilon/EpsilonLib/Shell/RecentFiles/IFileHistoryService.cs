using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IFileHistoryService
    {
        IEnumerable<FileHistoryRecord> RecentlyOpened { get; }
        Task RecordFileOpened(Guid editorProviderId, string filePath);
    }
}
