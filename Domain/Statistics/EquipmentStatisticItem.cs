using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Statistics
{
    public record EquipmentStatisticItem
    {
        public required string EquipmentType { get; set; }
        public required int Count { get; set; }
    }
}
