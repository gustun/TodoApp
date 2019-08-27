using AutoMapper;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Claims;
using TodoApp.Api.Infrastructure;
using TodoApp.Common;
using TodoApp.Common.Interface;
using TodoApp.DataAccess.Entities;

namespace TodoApp.Api.Test.Base
{
    public class BaseTest
    {
        protected string _testDataDirectory { get; private set; }
        protected User _sampleUser;
        protected ICryptoHelper _cyrptoHelper;
        protected IMapper _mapper;
        protected ClaimsPrincipal _userClaims;

        [SetUp]
        public virtual void Setup()
        {
            try
            {
                _testDataDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))), "TestData");
                _sampleUser = JsonConvert.DeserializeObject<User>(File.ReadAllText(Path.Combine(_testDataDirectory, "sampleUser.json")));
                _cyrptoHelper = new CryptoHelper();
                _sampleUser.Password = _cyrptoHelper.Hash(_sampleUser.Password);

                _mapper = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile(_cyrptoHelper));
                }).CreateMapper();

                _userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Sid, _sampleUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, _sampleUser.Email)
                }, "mock"));
            }
            catch
            {
                // Ignored
            }
        }
    }
}
