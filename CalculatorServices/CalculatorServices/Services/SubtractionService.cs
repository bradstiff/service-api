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
        private readonly IAuditRepository<SubtractionAuditRecord> _repository;

        public SubtractionService(IAuditRepository<SubtractionAuditRecord> repository, IHistoryClient historyService)
            :base(repository, historyService)
        {
            _repository = repository;
        }

        public Task<CalculatorResultViewModel> Subtract(Guid? id, decimal value, CancellationToken cancellationToken)
        {
            return Execute(id, Operations.Subtraction, value, cancellationToken);
        }
    }
}
