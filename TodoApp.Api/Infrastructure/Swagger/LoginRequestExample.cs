using Swashbuckle.AspNetCore.Filters;
using TodoApp.Api.ViewModels.Requests;

namespace TodoApp.Api.Infrastructure.Swagger
{
    public class LoginRequestExample : IExamplesProvider<LoginRequest>
    {
        public LoginRequest GetExamples()
        {
            return new LoginRequest
            {
                Email = "gokcan.ustun@yandex.com",
                Password = "1"
            };
        }
    }
}
