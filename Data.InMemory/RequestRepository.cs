using Data.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using static Domain.Request;

namespace Data.InMemory
{
    public class RequestRepository : IRequestRepository
    {
        private readonly List<Request> _requests;
        private int _nextId = 1;

        public RequestRepository()
        {
            _requests = new List<Request>();
            InitializeSampleData();
        }

        public List<Request> GetAll(RequestFilter filter = null)
        {
            if (_requests == null) return new List<Request>();

            var result = _requests.AsEnumerable();

            if (filter != null)
            {
                if (filter.StartDate.HasValue)
                    result = result.Where(r => r.Date >= filter.StartDate.Value);

                if (filter.EndDate.HasValue)
                    result = result.Where(r => r.Date <= filter.EndDate.Value);

                if (filter.Status.HasValue)
                    result = result.Where(r => r.Status == filter.Status.Value);

                if (!string.IsNullOrEmpty(filter.EquipmentType))
                    result = result.Where(r => r.EquipmentType == filter.EquipmentType);

                if (!string.IsNullOrEmpty(filter.Engineer))
                    result = result.Where(r => r.Engineer == filter.Engineer);
            }

            return result.OrderByDescending(r => r.Date).ToList();
        }
        private void InitializeSampleData()
        {
            if (_requests.Any()) return;

            try
            {
                var random = new Random();
                var equipmentTypes = new[] { "Компьютер", "Принтер", "Ноутбук", "Монитор", "Сервер", "МФУ" };
                var engineers = new[] { "Петров А.С.", "Сидоров В.К.", "Иванова М.П.", "Кузнецов Д.В.", null };
                var statuses = new[] { RequestStatus.New, RequestStatus.InProgress, RequestStatus.WaitingParts, RequestStatus.Completed, RequestStatus.Cancelled };

                var sampleRequests = new[]
                {
            new Request("REQ-001", DateTime.Now.AddDays(-5), "Компьютер", "Dell Optiplex",
                       "Не включается", RequestStatus.New, "Иванов Иван", "+7(999)123-45-67",
                       "Петров А.С.", "Требуется диагностика"),
            new Request("REQ-002", DateTime.Now.AddDays(-3), "Принтер", "HP LaserJet",
                       "Зажевывает бумагу", RequestStatus.InProgress, "Петрова Мария", "+7(999)765-43-21",
                       "Сидоров В.К.", "Заказаны ролики подачи")
        };

                foreach (var request in sampleRequests)
                {
                    request.Id = _nextId++;
                    _requests.Add(request);
                }

                for (int i = 0; i < 30; i++)
                {
                    var request = new Request(
                        $"REQ-{_nextId:000}",
                        DateTime.Now.AddDays(-random.Next(0, 180)),
                        equipmentTypes[random.Next(equipmentTypes.Length)],
                        $"Модель {random.Next(1000, 9999)}",
                        $"Описание проблемы {i + 1}",
                        statuses[random.Next(statuses.Length)],
                        $"Клиент {i + 1}",
                        $"+7(999){random.Next(1000000, 9999999)}",
                        engineers[random.Next(engineers.Length)],
                        $"Комментарий {i + 1}"
                    );

                    request.Id = _nextId++;
                    _requests.Add(request);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка инициализации: {ex.Message}");
            }
        }

        public int Add(Request request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.Id = _nextId++;
            _requests.Add(request);
            return request.Id;
        }

        public Request GetById(int id)
        {
            return _requests?.FirstOrDefault(r => r.Id == id);
        }

        public List<Request> GetAll()
        {
            return _requests?.OrderByDescending(r => r.Date).ToList() ?? new List<Request>();
        }

        public bool Update(Request request)
        {
            if (request == null || _requests == null) return false;

            var existing = GetById(request.Id);
            if (existing == null) return false;

            existing.Number = request.Number;
            existing.Date = request.Date;
            existing.EquipmentType = request.EquipmentType;
            existing.EquipmentModel = request.EquipmentModel;
            existing.ProblemDescription = request.ProblemDescription;
            existing.Status = request.Status;
            existing.ClientFullName = request.ClientFullName;
            existing.ClientPhone = request.ClientPhone;
            existing.Engineer = request.Engineer;
            existing.Comments = request.Comments;

            return true;
        }

        public bool Delete(int id)
        {
            if (_requests == null) return false;

            var request = GetById(id);
            return request != null && _requests.Remove(request);
        }

        public bool IsNumberExists(string number, int? excludeId = null)
        {
            if (_requests == null) return false;

            return excludeId.HasValue
                ? _requests.Any(r => r.Number == number && r.Id != excludeId.Value)
                : _requests.Any(r => r.Number == number);
        }

        public string GenerateRequestNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var randomPart = new Random().Next(1000, 9999);
            return $"REQ-{datePart}-{randomPart}";
        }
    }
}