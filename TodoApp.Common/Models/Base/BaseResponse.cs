namespace TodoApp.Common.Models.Base
{
    public class Result<T> : BaseResult
    {
        public T Data { get; set; } = default;
    }

    public class Result : BaseResult
    {
        public object Data { get; set; }
    }
}
