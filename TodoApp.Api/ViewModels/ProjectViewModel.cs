using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.ViewModels
{
    public class ProjectSimpleViewModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class ProjectSimpleResponse : ProjectSimpleViewModel
    {
        public Guid Id { get; set; }
    }

    public class ProjectResponse : ProjectSimpleResponse
    {
        public List<ProjectTaskViewModel> Tasks { get; set; } = new List<ProjectTaskViewModel>();
    }
}
