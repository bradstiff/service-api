using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data.Core
{
    public interface ICalculatorRepository
    {
        DbSet<CalculatorRecord> Calculators { get; set; }
        Task<CalculatorRecord> Find(Guid id, CancellationToken cancellationToken);
        Task Add(CalculatorRecord calculator, CancellationToken cancellationToken);
        Task Update(CalculatorRecord calculator, CancellationToken cancellationToken);
    }
}
