using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NUnit.Framework;
using System.IO;
using System.Text;
using TodoApp.Api.Controllers;
using TodoApp.Api.Infrastructure;
using TodoApp.Api.Infrastructure.Options;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Common;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DataAccess.Interface;
using Moq;
using System.Net;
using TodoApp.Api.ViewModels;

namespace Tests
{
    public class UserControllerTests
    {
        public string TestDataDirectory { get; private set; }
        public UsersController _userController { get; private set; }
        private const string Secretkey = "SECRETKEYGREATERTHAN128BITS";

        [SetUp]
        public void Setup()
        {
            TestDataDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))), "TestData");
            var sampleUserData = JsonConvert.DeserializeObject<User>(File.ReadAllText(Path.Combine(TestDataDirectory, "sampleUser.json")));
            var cyrptoHelper = new CryptoHelper();
            sampleUserData.Password = cyrptoHelper.Hash(sampleUserData.Password);

            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile(cyrptoHelper));
            }).CreateMapper();

            var jwtOptions = Options.Create(new JwtIssuerOptions()
            {
                Audience = "http://localhost:57837",
                Issuer = "http://localhost:57837",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secretkey)), SecurityAlgorithms.HmacSha256),
            });

            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByMail(sampleUserData.Email)).Returns(sampleUserData);
            mock.Setup(x => x.Save(It.Is<User>(y => y.Email != "gokcan.ustun@yandex.com"))).Returns(new Result());
            mock.Setup(x => x.Save(It.Is<User>(y => y.Email == "gokcan.ustun@yandex.com"))).Returns(new Result().AddError("This email is already being used by another user."));

            _userController = new UsersController(mock.Object, cyrptoHelper, mapper, jwtOptions);
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