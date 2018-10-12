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
        Task<CalculatorResultViewModel> Add(int? historyId, decimal vale, CancellationToken cancellationToken);
    }

    public class AdditionService : BaseCalculatorService<AdditionAuditRecord>, IAdditionService
    {
        private readonly ICalculatorRepository _repository;

        public AdditionService(ICalculatorRepository repository, IHistoryService historyService)
            : base(repository, historyService)
        {
            _repository = repository;
        }

        public Task<CalculatorResultViewModel> Add(int? historyId, decimal value, CancellationToken cancellationToken)
        {
            return Execute(historyId, Operations.Addition, value, cancellationToken);
        }

        protected override DbSet<AdditionAuditRecord> GetDbSet()
        {
            return _repository.AdditionAudits;
        }
    }
}
