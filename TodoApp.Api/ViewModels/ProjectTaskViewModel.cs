using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.ViewModels
{
    public class ProjectTaskViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }
    }

    public class ProjectTaskResponse : ProjectTaskViewModel
    {
        public Guid Id { get; set; }
    }
}
