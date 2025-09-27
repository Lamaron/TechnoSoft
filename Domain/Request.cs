namespace Domain
{
    public class Request
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Tipe { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public string Engineer { get; set; }
        public string Comments { get; set; }

        // Конструктор по умолчанию (обязателен для WPF)
        public Request()
        {
        }

        // Конструктор с параметрами
        public Request(string id, DateTime date, string tipe, string model, string description, string status, string clientfullname,
            string clientphone, string engineer, string comments)
        {
            Id = id;
            Date = date;
            Tipe = tipe;
            Model = model;
            Description = description;
            Status = status;
            ClientFullName = clientfullname;
            ClientPhone = clientphone;
            Engineer = engineer;
            Comments = comments;
        }
    }
}