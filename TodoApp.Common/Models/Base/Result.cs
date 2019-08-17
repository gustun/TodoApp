namespace TodoApp.Common.Models.Base
{
    public class Result : BaseResult
    {
        public Result() { }
        public Result(object data)
        {
            Data = data;
        }
        public object Data { get; set; }
    }
}
