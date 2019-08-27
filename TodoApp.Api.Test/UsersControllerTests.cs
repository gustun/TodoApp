using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;
using System.Text;
using TodoApp.Api.Controllers;
using TodoApp.Api.Infrastructure.Options;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Common;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DataAccess.Interface;
using Moq;
using System.Net;
using TodoApp.Api.Test.Base;

namespace Tests
{
    public class UsersControllerTests : BaseTest
    {
        private UsersController _userController { get; set; }
        private const string Secretkey = "SECRETKEYGREATERTHAN128BITS";

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var jwtOptions = Options.Create(new JwtIssuerOptions()
            {
                Audience = "http://localhost:57837",
                Issuer = "http://localhost:57837",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secretkey)), SecurityAlgorithms.HmacSha256),
            });

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByMail(_sampleUser.Email)).Returns(_sampleUser);
            userRepositoryMock.Setup(x => x.Save(It.Is<User>(y => y.Email != "gokcan.ustun@yandex.com"))).Returns(new Result());
            userRepositoryMock.Setup(x => x.Save(It.Is<User>(y => y.Email == "gokcan.ustun@yandex.com"))).Returns(new Result().AddError("This email is already being used by another user."));

            _userController = new UsersController(userRepositoryMock.Object, _cyrptoHelper, _mapper, jwtOptions);
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Test_Register(HttpStatusCode statusCode)
        {
            var request = new NewUserRequest
            {
                Email = statusCode == HttpStatusCode.OK 
                                        ? "test@yandex.com" 
                                        : "gokcan.ustun@yandex.com",
                Name = "test",
                Password = "1",
                Surname = "test"
            };

            var response = _userController.Register(request);
            response.Should().NotBeNull();
            var objectResult = (response as ObjectResult);
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(statusCode.ToInt());
        }

        [Test]
        [TestCase("gokcan.ustun@yandex.com", "1", HttpStatusCode.OK)]
        [TestCase("gokcan.ustun@yandex.com", "2", HttpStatusCode.BadRequest)]
        [TestCase("asdsads@yandex.com", "x", HttpStatusCode.NotFound)]
        public void Test_Login(string email, string password, HttpStatusCode statusCode)
        {
            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };
            var response = _userController.Login(loginRequest);
            response.Should().NotBeNull();
            var objectResult = (response as ObjectResult);
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(statusCode.ToInt());

            if (statusCode == HttpStatusCode.OK)
            {
                var model = objectResult.Value as Result;
                model.Should().NotBeNull();
                model.Data.Should().NotBeNull();
            }
        }
    }
}