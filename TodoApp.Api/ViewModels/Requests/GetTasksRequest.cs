using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Common.Models.Base;

namespace TodoApp.Api.ViewModels.Requests
{
    public class GetTasksRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public bool? IsCompleted { get; set; }
        public bool? IsImportant { get; set; }
        public RangeModel<DateTime?> DeadlineRange { get; set; } = new RangeModel<DateTime?>();
    }
}
