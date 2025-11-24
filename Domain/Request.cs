using System;

namespace Domain
{
    public class Request
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string EquipmentType { get; set; }
        public RequestStatus Status { get; set; }
        public string EquipmentModel { get; set; }
        public string ProblemDescription { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public string Engineer { get; set; }
        public string Comments { get; set; }

        public enum RequestStatus
        {
            New,           
            InProgress,    
            WaitingParts,  
            Ready,         
            Completed,    
            Cancelled      
        }

        public Request() { }

        public Request(string number, DateTime date, string equipmentType, string equipmentModel,
                      string problemDescription, RequestStatus status, string clientFullName,
                      string clientPhone, string engineer, string comments)
        {
            Number = number;
            Date = date;
            EquipmentType = equipmentType;
            EquipmentModel = equipmentModel;
            ProblemDescription = problemDescription;
            Status = status;
            ClientFullName = clientFullName;
            ClientPhone = clientPhone;
            Engineer = engineer;
            Comments = comments;
        }

        public void CopyFrom(Request other)
        {
            Number = other.Number;
            Date = other.Date;
            EquipmentType = other.EquipmentType;
            EquipmentModel = other.EquipmentModel;
            ProblemDescription = other.ProblemDescription;
            ClientFullName = other.ClientFullName;
            ClientPhone = other.ClientPhone;
            Status = other.Status;
            Engineer = other.Engineer;
            Comments = other.Comments;
        }
    }
}