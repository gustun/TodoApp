using TodoApp.Common.Models.Base;

namespace TodoApp.Api.ViewModels.Base
{
    public class ErrorResponse : Result
    {
        public string ErrorTraceId { get; set; }
    }
}
