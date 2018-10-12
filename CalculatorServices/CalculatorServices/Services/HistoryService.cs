using CalculatorServices.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorServices.Services
{
    public interface IHistoryService
    {
        Task<CalculatorHistoryViewModel> Get(Guid calculatorId, CancellationToken cancellationToken);
        Task<HistoryDto> GetLast(Guid calculatorId, CancellationToken cancellationToken);

        Task<HistoryDto> UpdateHistory(Guid calculatorId, Operations operation, decimal newValue, CancellationToken cancellationToken);
    }

    public class HistoryService : IHistoryService
    {
        public HistoryService(IConfiguration configuration)
        {
            var historyBaseAddress = configuration.GetValue<string>("HistoryServiceBaseAddress");
            this.HttpClient= new HttpClient() { BaseAddress = new Uri(historyBaseAddress) };
        }

        private HttpClient HttpClient { get; }

        private string GetKey(Guid calculatorId)
        {
            return $"Calculator[{calculatorId.ToString()}]";
        }

        public async Task<CalculatorHistoryViewModel> Get(Guid calculatorId, CancellationToken cancellationToken)
        {
            var result = await this.HttpClient.GetAsync(this.GetKey(calculatorId), cancellationToken);
            return (await result.Content.ReadAsAsync<IEnumerable<HistoryViewModel>>()).ToViewModel(calculatorId);
        }

        public async Task<HistoryDto> GetLast(Guid calculatorId, CancellationToken cancellationToken)
        {
            var result = await this.HttpClient.GetAsync($"{this.GetKey(calculatorId)}/last", cancellationToken);
            return (await result.Content.ReadAsAsync<HistoryViewModel>()).ToDto(calculatorId);
        }

        public async Task<HistoryDto> UpdateHistory(Guid calculatorId, Operations operation, decimal newValue, CancellationToken cancellationToken)
        {
            var history = new HistoryViewModel
            {
                Operation = operation.ToString(),
                NewValue = newValue.ToString()
            };
            var result = await this.HttpClient.PostAsync<HistoryViewModel>(this.GetKey(calculatorId), history, new JsonMediaTypeFormatter(), cancellationToken);
            return (await result.Content.ReadAsAsync<HistoryViewModel>()).ToDto(calculatorId);
        }
    }

    public class HistoryDto
    {
        public int Id { get; set; }

        public Guid CalculatorId { get; set; }

        public decimal? OldValue { get; set; }

        public Operations Operation { get; set; }

        public decimal NewValue { get; set; }
    }

    internal static class HistoryMapper
    {
        internal static HistoryDto ToDto(this HistoryViewModel viewModel, Guid calculatorId)
        {
            return new HistoryDto
            {
                Id = viewModel.Id,
                CalculatorId = calculatorId,
                Operation = (Operations)Enum.Parse(typeof(Operations), viewModel.Operation),
                OldValue = viewModel.OldValue != null ? decimal.Parse(viewModel.OldValue) : new decimal?(),
                NewValue = decimal.Parse(viewModel.NewValue)
            };
        }

        internal static CalculatorHistoryViewModel ToViewModel(this IEnumerable<HistoryViewModel> viewModels, Guid calculatorID)
        {
            return new CalculatorHistoryViewModel
            {
                Id = calculatorID,
                Operations = viewModels.Select(viewModel => new OperationViewModel
                {
                    Operation = viewModel.Operation.ToString(),
                    OldValue = viewModel.OldValue != null ? decimal.Parse(viewModel.OldValue) : new decimal?(),
                    NewValue = decimal.Parse(viewModel.NewValue)
                })
            };
        }
    }
}