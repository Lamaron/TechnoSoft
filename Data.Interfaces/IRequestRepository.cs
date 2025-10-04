using System.Collections.Generic;
using Domain;

namespace Data.Interfaces
{
    public interface IRequestRepository
    {
        int Add(Request request);
        Request GetById(int id);
        List<Request> GetAll();
        bool Update(Request request);
        bool Delete(int id);
        bool IsNumberExists(string number, int? excludeId = null);
        string GenerateRequestNumber(); // Добавляем этот метод
    }
}