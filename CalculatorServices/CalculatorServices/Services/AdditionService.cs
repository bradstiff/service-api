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
    public interface IAdditionService
    {
        Task<CalculatorResultViewModel> Add(Guid? id, decimal vale, CancellationToken cancellationToken);
    }

    public class AdditionService : BaseCalculatorService<AdditionAuditRecord>, IAdditionService
    {
        private readonly IAuditRepository<AdditionAuditRecord> _repository;

        public AdditionService(IAuditRepository<AdditionAuditRecord> repository, IHistoryClient historyService)
            : base(repository, historyService)
        {
            _repository = repository;
        }

        public Task<CalculatorResultViewModel> Add(Guid? id, decimal value, CancellationToken cancellationToken)
        {
            return Execute(id, Operations.Addition, value, cancellationToken);
        }
    }
}
