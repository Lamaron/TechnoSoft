using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Statistics
{
    public class StatusStatisticItem
    {
        public required Request.RequestStatus Status { get; set; }
        public required int Count { get; set; }
    }
}
