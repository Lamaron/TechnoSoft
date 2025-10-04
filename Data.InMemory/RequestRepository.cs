using System;
using System.Collections.Generic;
using System.Linq;
using Data.Interfaces;
using Domain;

namespace Data.InMemory
{
    public class RequestRepository : IRequestRepository
    {
        private readonly List<Request> _requests = new List<Request>();
        private int _nextId = 1;

        public RequestRepository()
        {
            // Тестовые данные
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            _requests.AddRange(new[]
            {
                new Request("001", DateTime.Now.AddDays(-5), "Компьютер", "Dell Optiplex",
                           "Не включается", "Новая заявка", "Иванов Иван", "+7(999)123-45-67",
                           "Петров А.С.", "Требуется диагностика"),

                new Request("002", DateTime.Now.AddDays(-3), "Принтер", "HP LaserJet",
                           "Зажевывает бумагу", "В процессе ремонта", "Петрова Мария", "+7(999)765-43-21",
                           "Сидоров В.К.", "Заказаны ролики подачи"),

                new Request("003", DateTime.Now.AddDays(-1), "Ноутбук", "Lenovo ThinkPad",
                           "Не работает Wi-Fi", "Ожидание запчастей", "Сидоров Алексей", "+7(999)555-44-33",
                           "Петров А.С.", "Ожидается сетевая карта")
            });

            // Устанавливаем ID для тестовых данных
            foreach (var request in _requests)
            {
                request.Id = _nextId++;
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
            return _requests.FirstOrDefault(r => r.Id == id);
        }

        public List<Request> GetAll()
        {
            return _requests.OrderByDescending(r => r.Date).ToList();
        }

        public bool Update(Request request)
        {
            if (request == null)
                return false;

            var existingRequest = GetById(request.Id);
            if (existingRequest == null)
                return false;

            // Обновляем все поля кроме Id
            existingRequest.Number = request.Number;
            existingRequest.Date = request.Date;
            existingRequest.EquipmentType = request.EquipmentType;
            existingRequest.EquipmentModel = request.EquipmentModel;
            existingRequest.ProblemDescription = request.ProblemDescription;
            existingRequest.Status = request.Status;
            existingRequest.ClientFullName = request.ClientFullName;
            existingRequest.ClientPhone = request.ClientPhone;
            existingRequest.Engineer = request.Engineer;
            existingRequest.Comments = request.Comments;

            return true;
        }

        public bool Delete(int id)
        {
            var request = GetById(id);
            if (request == null)
                return false;

            return _requests.Remove(request);
        }
    }
}