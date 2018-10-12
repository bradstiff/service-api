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

        public async Task<decimal?> GetLatestValue(Guid calculatorId, CancellationToken cancellationToken)
        {
            return (await this.HistoryService.GetLast(calculatorId, cancellationToken))?.NewValue;
        }

        protected async Task WriteToGlobalHistory(Guid calculatorId, Operations operation, decimal newValue, CancellationToken cancellationToken)
        {
            await this.HistoryService.UpdateHistory(calculatorId, operation, newValue, cancellationToken);
        }

        protected abstract DbSet<T> GetDbSet();

        protected async Task<CalculatorResultViewModel> Execute(Guid? id, Operations operation, decimal value,
            CancellationToken cancellationToken)
        {
            decimal oldValue = 0;
            if (id.HasValue)
            {
                oldValue = (await GetLatestValue(id.Value, cancellationToken)).GetValueOrDefault(0);
            }
            else
            {
                id = Guid.NewGuid();
            }

            var result = CalculateResult(operation, oldValue, value);

            var audit = new T()
            {
                CalculatorId = id.Value,
                NewValue = result,
                OldValue = oldValue
            };

            var dbSet = GetDbSet();

            await dbSet.AddAsync(audit, cancellationToken);

            await Repository.SaveChangesAsync(cancellationToken);

            //id = await WriteToGlobalHistory(id, operation, value, result, cancellationToken);
            await WriteToGlobalHistory(id.Value, operation, result, cancellationToken);

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