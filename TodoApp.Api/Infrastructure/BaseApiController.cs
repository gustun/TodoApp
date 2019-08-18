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

        protected bool CheckAuthorization(Guid userId, Guid? authenticatedUserId = null)
        {
            if (authenticatedUserId == null)
                authenticatedUserId = GetUserId();

            return userId == authenticatedUserId.Value || IsAdmin(authenticatedUserId.Value);
        }

        private bool IsAdmin(Guid authenticatedUserId)
        {
            return false;
        }
    }
}
