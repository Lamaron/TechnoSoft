using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public record RequestFilter
    {
        public static RequestFilter Empty => new();

        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public Request.RequestStatus? Status { get; init; }
        public string? EquipmentType { get; init; }
        public string? Engineer { get; init; }
    }
}
