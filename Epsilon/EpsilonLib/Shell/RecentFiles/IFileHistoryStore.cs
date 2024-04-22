using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IFileHistoryStore
    {
        Task StoreRecords(IEnumerable<FileHistoryRecord> records);
        Task<IEnumerable<FileHistoryRecord>> FetchRecords();
    }
}
