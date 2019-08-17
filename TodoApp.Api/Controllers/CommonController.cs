using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TodoApp.Api.Infrastructure;
using TodoApp.Api.ViewModels.Responses;
using TodoApp.DataAccess.Interface;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    public class CommonController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CommonController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous, HttpGet, Route("~/v1/health-check")]
        public ActionResult Get()
        {
            Log.Information("Logger test...");
            return Ok();
        }

        [HttpGet, Route("~/v1/me")]
        public IActionResult GetUserFromToken()
        {
            var userId = GetUserId();
            var user = _userRepository.GetById(userId);
            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserViewModel>(user));
        }
    }
}
