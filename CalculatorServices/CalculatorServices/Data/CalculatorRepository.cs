using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using CalculatorServices.Data.Core;

namespace CalculatorServices.Data
{
    public class CalculatorRepository : DbContext, ICalculatorRepository
    {
        public CalculatorRepository(DbContextOptions<CalculatorRepository> options)
            : base(options)
        { }

        public DbSet<CalculatorRecord> Calculators { get; set; }

        public async Task<CalculatorRecord> Find(Guid id, CancellationToken cancellationToken)
        {
            return await this.Calculators.FindAsync(keyValues: new[] { (object)id }, cancellationToken: cancellationToken);
        }

        public async Task Add(CalculatorRecord calculator, CancellationToken cancellationToken)
        {
            this.Calculators.Add(calculator);
            await this.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(CalculatorRecord calculator, CancellationToken cancellationToken)
        {
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}
