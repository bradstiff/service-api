using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HistoryServices.Data.Core
{
    public interface IHistoryRepository
    {
        DbSet<HistoryRecord> Histories { get; set; }
        Task<IEnumerable<HistoryRecord>> GetAll(string key, CancellationToken cancellationToken);
        Task<HistoryRecord> GetLast(string key, CancellationToken cancellationToken);
        void Insert(HistoryRecord history);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
