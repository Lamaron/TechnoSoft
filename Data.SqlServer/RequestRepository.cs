using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace Data.SqlServer
{
    public class RequestRepository : IRequestRepository
    {
        private readonly TechnoSoftDbContext _context;

        public RequestRepository(TechnoSoftDbContext context)
        {
            _context = context;
        }

        public int Add(Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
            return request.Id;
        }

        public Request GetById(int id)
        {
            return _context.Requests.Find(id);
        }

        public List<Request> GetAll(RequestFilter filter = null)
        {
            var query = _context.Requests.AsQueryable();

            if (filter != null)
            {
                if (filter.StartDate.HasValue)
                    query = query.Where(r => r.Date >= filter.StartDate.Value);

                if (filter.EndDate.HasValue)
                    query = query.Where(r => r.Date <= filter.EndDate.Value);

                if (filter.Status.HasValue)
                    query = query.Where(r => r.Status == filter.Status.Value);

                if (!string.IsNullOrEmpty(filter.EquipmentType))
                    query = query.Where(r => r.EquipmentType == filter.EquipmentType);

                if (!string.IsNullOrEmpty(filter.Engineer))
                    query = query.Where(r => r.Engineer == filter.Engineer);
            }

            return query.OrderByDescending(r => r.Date).ToList();
        }

        public bool Update(Request request)
        {
            var existing = _context.Requests.Find(request.Id);
            if (existing == null)
                return false;

            CopyRequestProperties(request, existing);

            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var request = _context.Requests.Find(id);
            if (request == null)
                return false;

            _context.Requests.Remove(request);
            _context.SaveChanges();
            return true;
        }

        public bool IsNumberExists(string number, int? excludeId = null)
        {
            return excludeId.HasValue
                ? _context.Requests.Any(r => r.Number == number && r.Id != excludeId.Value)
                : _context.Requests.Any(r => r.Number == number);
        }

        public string GenerateRequestNumber()
        {
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var randomPart = new Random().Next(1000, 9999);
            return $"REQ-{datePart}-{randomPart}";
        }

        private void CopyRequestProperties(Request source, Request destination)
        {
            destination.Number = source.Number;
            destination.Date = source.Date;
            destination.EquipmentType = source.EquipmentType;
            destination.EquipmentModel = source.EquipmentModel;
            destination.ProblemDescription = source.ProblemDescription;
            destination.ClientFullName = source.ClientFullName;
            destination.ClientPhone = source.ClientPhone;
            destination.Status = source.Status;
            destination.Engineer = source.Engineer;
            destination.Comments = source.Comments;
        }
    }
}
