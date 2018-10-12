using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data.Core
{
    public interface IAuditRepository<T>
        where T : BaseOperationAuditRecord, new()
    {
        DbSet<T> Audits { get; set; }

        Task AddAudit(T auditRecord, CancellationToken cancellationToken);
    }
}