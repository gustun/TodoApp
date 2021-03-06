﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApp.Api.Infrastructure;
using TodoApp.Api.Infrastructure.Options;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Common.Interface;
using TodoApp.DataAccess.Entities;
using TodoApp.DataAccess.Interface;
using TodoApp.Common.Models.Base;
using TodoApp.Api.ViewModels;

namespace TodoApp.Api.Controllers
{
    [Route("v1/users")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoHelper _cryptoHelper;
        private readonly IMapper _mapper;
        private readonly JwtIssuerOptions _jwtOptions;

        public UsersController(IUserRepository userRepository,
            ICryptoHelper cryptoHelper,
            IMapper mapper,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userRepository = userRepository;
            _cryptoHelper = cryptoHelper;
            _mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Generates an authentication token
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost, Route("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var toReturn = new Result();
            var user = _userRepository.GetByMail(loginRequest.Email);
            if (user == null)
                return NotFound(toReturn.AddError("User not found."));

            if (user.Password != _cryptoHelper.Hash(loginRequest.Password))
                return BadRequest(toReturn.AddError("Password is not correct."));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Expires = _jwtOptions.Expiration,
                NotBefore = _jwtOptions.NotBefore,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Sid,user.Id.ToString())
                }),
                SigningCredentials = _jwtOptions.SigningCredentials
            });

            toReturn.Data = new { UserToken = handler.WriteToken(token) };
            return Ok(toReturn);
        }


        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost, Route("register")]
        public IActionResult Register(NewUserRequest newUser)
        {
            var result = _userRepository.Save(_mapper.Map<User>(newUser));
            if (!result.IsSuccess)
                return BadRequest(result);

            result.Data = _mapper.Map<UserViewModel>(result.Data);
            return Ok(result);
        }
    }
}
