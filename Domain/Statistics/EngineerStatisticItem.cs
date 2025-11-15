using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Statistics
{
    public class EngineerStatisticItem
    {
        public required string EngineerName { get; set; }
        public required int Count { get; set; }
        public required int Completed { get; set; }
    }
}
