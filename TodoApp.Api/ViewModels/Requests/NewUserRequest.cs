using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.ViewModels.Requests
{
    public class NewUserRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
    }
}
