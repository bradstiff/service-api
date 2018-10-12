using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculatorServices.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data
{
    public class CalculatorRepository<T> : DbContext, ICalculatorRepository<T>
        where T : BaseOperationAuditRecord, new()
    {
        public DbSet<T> Audits { get; set; }

        public CalculatorRepository(DbContextOptions<CalculatorRepository<T>> options)
            : base(options)
        { }

        public async Task Add(T auditRecord)
        {
            this.Audits.Add(auditRecord);
            await this.SaveChangesAsync();
        }
    }
}
