using Data.Interfaces;
using Domain;
using Domain.Statistics;
using System.Collections.Generic;
using System.Linq;
using static Domain.Request;

namespace Services
{
    public class StatisticsService
    {
        private readonly IRequestRepository _requestRepository;

        public StatisticsService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Получить распределение заявок по статусам
        /// </summary>
        public List<StatusStatisticItem> GetRequestsByStatus(RequestFilter filter = null)
        {
            var requests = _requestRepository.GetAll(filter);

            return requests
                .GroupBy(r => r.Status)
                .Select(g => new StatusStatisticItem
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .OrderBy(s => s.Status)
                .ToList();
        }

        /// <summary>
        /// Получить динамику заявок по месяцам
        /// </summary>
        public List<MonthStatisticItem> GetRequestsByMonth(RequestFilter filter = null)
        {
            var requests = _requestRepository.GetAll(filter);

            return requests
                .GroupBy(r => new { r.Date.Year, r.Date.Month })
                .Select(g => new MonthStatisticItem
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();
        }

        /// <summary>
        /// Получить статистику по инженерам
        /// </summary>
        public List<EngineerStatisticItem> GetRequestsByEngineer(RequestFilter filter = null)
        {
            var requests = _requestRepository.GetAll(filter);

            return requests
                .Where(r => !string.IsNullOrEmpty(r.Engineer))
                .GroupBy(r => r.Engineer)
                .Select(g => new EngineerStatisticItem
                {
                    EngineerName = g.Key,
                    Count = g.Count(),
                    Completed = g.Count(r => r.Status == RequestStatus.Completed)
                })
                .OrderByDescending(e => e.Count)
                .ToList();
        }

        /// <summary>
        /// Получить статистику по типам оборудования
        /// </summary>
        public List<EquipmentStatisticItem> GetRequestsByEquipment(RequestFilter filter = null)
        {
            var requests = _requestRepository.GetAll(filter);

            return requests
                .GroupBy(r => r.EquipmentType)
                .Select(g => new EquipmentStatisticItem
                {
                    EquipmentType = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(e => e.Count)
                .ToList();
        }
    }
}