using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HistoryServices.ViewModels
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public string Operation { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
