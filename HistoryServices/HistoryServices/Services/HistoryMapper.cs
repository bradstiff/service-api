using HistoryServices.Data;
using HistoryServices.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoryServices.Services
{
    internal static class HistoryMapper
    {
        internal static HistoryViewModel ToViewModel(this HistoryRecord historyRecord)
        {
            return new HistoryViewModel
            {
                Id = historyRecord.Id,
                Operation = historyRecord.Operation,
                OldValue = historyRecord.OldValue,
                NewValue = historyRecord.NewValue
            };
        }

        internal static HistoryRecord ToRecord(this HistoryViewModel viewModel)
        {
            return new HistoryRecord
            {
                Id = viewModel.Id,
                Operation = viewModel.Operation,
                OldValue = viewModel.OldValue,
                NewValue = viewModel.NewValue
            };
        }
    }
}
