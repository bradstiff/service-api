using System.Collections.Generic;

namespace CalculatorServices.ViewModels
{
    public class CalculatorHistoryViewModel
    {
        public int Id { get; set; }

        public IEnumerable<OperationViewModel> Operations { get; set; }
    }
}