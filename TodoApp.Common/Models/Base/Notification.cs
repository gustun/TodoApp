using TodoApp.Common.Enums;

namespace TodoApp.Common.Models.Base
{
    public class Notification
    {
        public ENotificationType NotificationType { get; set; }
        public string Message { get; set; }
    }
}
