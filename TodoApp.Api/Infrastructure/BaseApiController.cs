using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace TodoApp.Api.Infrastructure
{
    public class BaseApiController : ControllerBase
    {
        protected Guid GetUserId()
        {
            Guid.TryParse(User.Claims.First(i => i.Type == ClaimTypes.Sid).Value, out var userId);
            return userId;
        }
    }
}
