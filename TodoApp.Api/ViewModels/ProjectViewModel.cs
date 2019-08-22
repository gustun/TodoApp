using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.ViewModels
{
    public class ProjectCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class ProjectViewModel : ProjectCreateViewModel
    {
        public Guid Id { get; set; }
    }

    public class ProjectAndTasksViewModel : ProjectViewModel
    {
        public List<ProjectTaskCreateViewModel> Tasks { get; set; } = new List<ProjectTaskCreateViewModel>();
    }
}
