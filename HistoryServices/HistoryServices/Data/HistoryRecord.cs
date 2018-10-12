using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoryServices.Data
{
    public class HistoryRecord
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Operation { get; set; }
        public DateTime Timestamp { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
