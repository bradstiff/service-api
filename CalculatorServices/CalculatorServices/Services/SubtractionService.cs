using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Data;
using CalculatorServices.Data.Core;
using CalculatorServices.Services.Core;
using CalculatorServices.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Services
{
    public interface ISubtractionService
    {
        Task<CalculatorResultViewModel> Subtract(Guid? id, decimal value, CancellationToken cancellationToken);
    }

    public class SubtractionService : BaseCalculatorService<SubtractionAuditRecord>, ISubtractionService
    {
        public SubtractionService(ICalculatorRepository calculatorRepository, IAuditRepository<SubtractionAuditRecord> auditRepository, IHistoryClient historyService)
            : base(calculatorRepository, auditRepository, historyService)
        {
        }

        public Task<CalculatorResultViewModel> Subtract(Guid? id, decimal value, CancellationToken cancellationToken)
        {
            return Execute(id, Operations.Subtraction, value, cancellationToken);
        }
    }
}
