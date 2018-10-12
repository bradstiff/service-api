using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HistoryServices.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace HistoryServices.Data
{
    public class HistoryRepository : DbContext, IHistoryRepository
    {
        private DbSet<HistoryRecord> Histories { get; set; }

        public HistoryRepository(DbContextOptions options)
            : base(options)
        { }

        public async Task<IEnumerable<HistoryRecord>> GetAll(string key, CancellationToken cancellationToken)
        {
            return await this.Histories.Where(h => h.Key == key).ToListAsync(cancellationToken);
        }

        public async Task<HistoryRecord> GetLast(string key, CancellationToken cancellationToken)
        {
            return await this.Histories.Where(h => h.Key == key).OrderByDescending(h => h.Id).FirstOrDefaultAsync(cancellationToken);
        }

        public void Insert(HistoryRecord history)
        {
            this.Histories.Add(history);
        }
    }
}
