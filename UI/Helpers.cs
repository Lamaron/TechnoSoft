using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Request;
namespace UI.Helpers
{
    public class StatusItem
    {
        public RequestStatus Value { get; set; }
        public string DisplayText { get; set; }

        public StatusItem(RequestStatus value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }
    }

    public static class StatusHelper
    {
        public static List<StatusItem> GetStatusItems()
        {
            return new List<StatusItem>
            {
                new StatusItem(RequestStatus.New, "Новая заявка"),
                new StatusItem(RequestStatus.InProgress, "В процессе ремонта"),
                new StatusItem(RequestStatus.WaitingParts, "Ожидание запчастей"),
                new StatusItem(RequestStatus.Ready, "Готово к выдаче"),
                new StatusItem(RequestStatus.Completed, "Завершена"),
                new StatusItem(RequestStatus.Cancelled, "Отменена")
            };
        }

        public static string GetDisplayText(RequestStatus status)
        {
            var item = GetStatusItems().FirstOrDefault(s => s.Value == status);
            return item?.DisplayText ?? status.ToString();
        }

        public static RequestStatus GetStatusByDisplayText(string displayText)
        {
            var item = GetStatusItems().FirstOrDefault(s => s.DisplayText == displayText);
            return item?.Value ?? RequestStatus.New;
        }
    }
}