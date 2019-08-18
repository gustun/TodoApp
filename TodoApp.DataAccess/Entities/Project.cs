using System.Collections.Generic;
using TodoApp.DataAccess.Entities.Base;

namespace TodoApp.DataAccess.Entities
{
    public class Project : CouchbaseEntity<Project>
    {
        public string Name { get; set; }
        public List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
    }
}
