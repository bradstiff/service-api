using HistoryServices.Data;
using HistoryServices.Data.Core;
using HistoryServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HistoryServices.Services
{
    public interface IHistoryService
    {
        Task<IEnumerable<HistoryViewModel>> GetAll(string key, CancellationToken cancellationToken);
        Task<HistoryViewModel> GetLast(string key, CancellationToken cancellationToken);
        Task<HistoryViewModel> Add(string key, HistoryViewModel viewModel, CancellationToken cancellationToken);
    }

    public class HistoryService : IHistoryService
    {
        private IHistoryRepository HistoryRepository { get; }

        public HistoryService(IHistoryRepository historyRepository)
        {
            this.HistoryRepository = historyRepository;
        }

        public async Task<HistoryViewModel> Add(string key, HistoryViewModel viewModel, CancellationToken cancellationToken)
        {
            var record = viewModel.ToRecord();
            record.Key = key;
            record.Timestamp = DateTime.Now;
            var last = await HistoryRepository.GetLast(key, cancellationToken);
            if (last != null)
            {
                record.OldValue = last.NewValue;
            }

            HistoryRepository.Insert(record);
            await HistoryRepository.SaveChangesAsync(cancellationToken);
            return record.ToViewModel();
        }

        public async Task<IEnumerable<HistoryViewModel>> GetAll(string key, CancellationToken cancellationToken)
        {
            return (await HistoryRepository.GetAll(key, cancellationToken))
                .Select(r => r.ToViewModel());
        }

        public async Task<HistoryViewModel> GetLast(string key, CancellationToken cancellationToken)
        {
            return (await HistoryRepository.GetLast(key, cancellationToken)).ToViewModel();
        }
    }
}
