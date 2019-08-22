using System;
using System.Collections.Generic;

namespace TodoApp.Api.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<ProjectAndTasksViewModel> Projects { get; set; } = new List<ProjectAndTasksViewModel>();
    }
}
