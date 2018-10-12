using System;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Data;
using CalculatorServices.Data.Core;
using CalculatorServices.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Services.Core
{
    public abstract class BaseCalculatorService<T>
        where T : BaseOperationAuditRecord, new()
    {
        protected IAuditRepository<T> AuditRepository { get; }
        protected ICalculatorRepository CalculatorRepository { get; }

        protected IHistoryClient HistoryService { get; }

        protected BaseCalculatorService(ICalculatorRepository calculatorRepository, IAuditRepository<T> auditRepository, IHistoryClient historyService)
        {
            CalculatorRepository = calculatorRepository;
            AuditRepository = auditRepository;
            HistoryService = historyService;
        }

        protected async Task<CalculatorResultViewModel> Execute(Guid? id, Operations operation, decimal value, CancellationToken cancellationToken)
        {
            var isNew = id == null;
            CalculatorRecord calculator;
            if (isNew)
            {
                calculator = new CalculatorRecord
                {
                    Id = Guid.NewGuid(),
                    Value = 0,
                };
            }
            else
            {
                calculator = await this.CalculatorRepository.Find(id.Value, cancellationToken);
            }

            var oldValue = calculator.Value;
            var result = CalculateResult(operation, oldValue, value);
            calculator.Value = result;

            var audit = new T()
            {
                CalculatorId = calculator.Id,
                NewValue = result,
                OldValue = oldValue
            };

            Task repositoryTask;
            if (isNew)
            {
                repositoryTask = this.CalculatorRepository.Add(calculator, cancellationToken);
            }
            else
            {
                repositoryTask = this.CalculatorRepository.Update(calculator, cancellationToken);
            }
            var auditTask = this.AuditRepository.AddAudit(audit, cancellationToken);
            var historyTask = this.HistoryService.UpdateHistory(calculator.Id, operation, result, cancellationToken);

            await Task.WhenAll(repositoryTask, auditTask, historyTask);

            return new CalculatorResultViewModel() { GlobalId = calculator.Id, Result = result };
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