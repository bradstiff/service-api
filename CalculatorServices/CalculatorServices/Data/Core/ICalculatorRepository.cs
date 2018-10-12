using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data.Core
{
    public interface ICalculatorRepository
    {
        DbSet<AdditionAuditRecord> AdditionAudits { get; set; }

        DbSet<SubtractionAuditRecord> SubtractionAudits { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}