using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using TodoApp.Api.Infrastructure;
using TodoApp.Api.ViewModels;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Interface;

namespace TodoApp.Api.Controllers
{
    [ApiController]
    public class CommonController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CommonController( IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            //_userRepository = userRepository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous, HttpGet, Route("~/v1/health-check")]
        public ActionResult Get()
        {
            Log.Information("Logger test...");
            return Ok(new Result( new
            {
                _hostingEnvironment.ApplicationName, 
                _hostingEnvironment.EnvironmentName
            }));
        }

        [HttpGet, Route("~/v1/me")]
        public IActionResult GetUserFromToken()
        {
            var userId = GetUserId();
            var user = _userRepository.GetById(userId);
            if (user == null)
                return NotFound();

            return Ok(new Result(_mapper.Map<UserViewModel>(user)));
        }
    }
}
