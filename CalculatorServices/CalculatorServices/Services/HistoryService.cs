using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorServices.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryDto>> Get(int calculatorId, CancellationToken cancellationToken);

        Task<HistoryDto> UpdateHistory(int? calculatorId, Operations operation, decimal newValue, CancellationToken cancellationToken);
    }

    public class HistoryService : IHistoryService
    {
        public async Task<IEnumerable<HistoryDto>> Get(int calculatorId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException($"HistoryService.Get should return the calculator history for the {nameof(calculatorId)}");
        }

        public async Task<HistoryDto> UpdateHistory(int? calculatorId, Operations operation, decimal newValue, CancellationToken cancellationToken)
        {
            throw new NotImplementedException($"HistoryService.UpdateHistory should add to the calculator history for the {nameof(calculatorId)}");
        }
    }

    public class HistoryDto
    {
        public int Id { get; set; }

        public int CalculatorId { get; set; }

        public decimal? OldValue { get; set; }

        public Operations Operation { get; set; }

        public decimal NewValue { get; set; }
    }

    public class HistoryEntity
    {
        public int Id { get; set; }

        public int CalculatorId { get; set; }

        public decimal OldValue { get; set; }

        public Operations Operation { get; set; }

        public decimal NewValue { get; set; }
    }

    public enum Operations
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}
