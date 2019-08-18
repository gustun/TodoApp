using System;
using TodoApp.DataAccess.Entities.Base;

namespace TodoApp.DataAccess.Entities
{
    public class ProjectTask : CouchbaseEntity<ProjectTask>
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }

    }
}
