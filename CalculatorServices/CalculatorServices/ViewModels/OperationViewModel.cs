namespace CalculatorServices.ViewModels
{
    public class OperationViewModel
    {
        public string Operation { get; set; }

        public decimal? OldValue { get; set; }

        public decimal NewValue { get; set; }
    }
}