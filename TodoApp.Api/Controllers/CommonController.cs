using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Api.Infrastructure;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseApiController
    {
        [AllowAnonymous, HttpGet, Route("~/v1/health-check")]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpGet, Route("~/v1/me")]
        public IActionResult GetUserFromToken()
        {
            //todo: ...
            return NoContent();
        }
    }
}
