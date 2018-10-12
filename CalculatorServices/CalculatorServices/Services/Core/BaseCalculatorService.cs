using System;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Data.Core;
using CalculatorServices.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Services.Core
{
    public abstract class BaseCalculatorService<T>
        where T : BaseOperationAuditRecord, new()
    {
        protected ICalculatorRepository Repository { get; }

        protected IHistoryService HistoryService { get; }

        protected BaseCalculatorService(ICalculatorRepository repository, IHistoryService historyService)
        {
            Repository = repository;
            HistoryService = historyService;
        }

        public async Task<decimal?> GetLatestValue(int calculatorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Implement GetLatestValue using the HistoryService you wrote");
        }

        protected async Task<int> WriteToGlobalHistory(int? calculatorId, Operations operation, decimal value, decimal newValue, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("Implement WriteToGlobalHistory using the HistoryService you wrote");
        }

        protected abstract DbSet<T> GetDbSet();

        protected async Task<CalculatorResultViewModel> Execute(int? id, Operations operation, decimal value,
            CancellationToken cancellationToken)
        {
            decimal oldValue = 0;
            if (id.HasValue)
            {
                oldValue = (await GetLatestValue(id.Value, cancellationToken)).GetValueOrDefault(0);
            }

            var result = CalculateResult(operation, oldValue, value);

            var audit = new T()
            {
                NewValue = result,
                OldValue = oldValue
            };

            var dbSet = GetDbSet();

            await dbSet.AddAsync(audit, cancellationToken);

            await Repository.SaveChangesAsync(cancellationToken);

            id = await WriteToGlobalHistory(id, operation, value, result, cancellationToken);

            return new CalculatorResultViewModel() { GlobalId = id.Value, Result = result };
        }

        private decimal CalculateResult(Operations operation, decimal oldValue, decimal value)
        {
            switch (operation)
            {
                case Operations.Addition:
                    return oldValue + value;
                case Operations.Subtraction:
                    return oldValue - value;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
    }
}