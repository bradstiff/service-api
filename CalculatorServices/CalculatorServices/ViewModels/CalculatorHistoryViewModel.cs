using System;
using System.Collections.Generic;

namespace CalculatorServices.ViewModels
{
    public class CalculatorHistoryViewModel
    {
        public Guid Id { get; set; }

        public IEnumerable<OperationViewModel> Operations { get; set; }
    }
}