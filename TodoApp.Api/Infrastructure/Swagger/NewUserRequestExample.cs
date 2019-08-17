using Swashbuckle.AspNetCore.Filters;
using TodoApp.Api.ViewModels.Requests;

namespace TodoApp.Api.Infrastructure.Swagger
{
    public class NewUserRequestExample : IExamplesProvider<NewUserRequest>
    {
        public NewUserRequest GetExamples()
        {
            return new NewUserRequest
            {
                Name = "Gokcan",
                Surname = "Ustun",
                Email = "gokcan.ustun@yandex.com",
                Password = "1"
            };
        }
    }
}
