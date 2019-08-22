using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.ViewModels
{
    public class ProjectTaskCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsImportant { get; set; }
        public DateTime? Deadline { get; set; }
    }

    public class ProjectTaskViewModel : ProjectTaskCreateViewModel
    {
        public Guid Id { get; set; }
    }
}
