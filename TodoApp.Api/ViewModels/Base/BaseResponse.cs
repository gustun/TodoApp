using TodoApp.Common.Models.Base;

namespace TodoApp.Api.ViewModels.Base
{
    public class BaseResponse<T> : Result
    {
        public T Data { get; set; } = default(T);
    }

    public class BaseResponse : Result
    {
        public object Data { get; set; }
    }
}
