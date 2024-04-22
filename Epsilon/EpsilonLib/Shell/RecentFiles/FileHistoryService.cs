using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class FileHistoryService : IFileHistoryService
    {
        private readonly IFileHistoryStore _store;
        private List<FileHistoryRecord> _records;

        public IEnumerable<FileHistoryRecord> RecentlyOpened => _records;

        [ImportingConstructor]
        public FileHistoryService(IFileHistoryStore store)
        {
            _store = store;
        }

        public async Task InitAsync()
        {
            _records = new List<FileHistoryRecord>((await _store.FetchRecords()).OrderByDescending(x => x.LastOpened));
        }

        public async Task RecordFileOpened(Guid editorProviderId, string filePath)
        {
            var record = _records.FirstOrDefault(x => x.FilePath == filePath);
            _records.Remove(record);
            record = new FileHistoryRecord(editorProviderId, filePath);
            _records.Insert(0, record);
            await _store.StoreRecords(_records);
        }
    }
}
