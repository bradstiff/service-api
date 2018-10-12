using System;

namespace CalculatorServices.Data.Core
{
    public abstract class BaseOperationAuditRecord
    {
        public int Id { get; set; }
        public Guid CalculatorId { get; set; }

        public decimal OldValue { get; set; }

        public decimal NewValue { get; set; }
    }
}