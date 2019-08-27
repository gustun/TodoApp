using FluentAssertions;
using NUnit.Framework;
using TodoApp.Api.Controllers;
using TodoApp.Common;
using Microsoft.AspNetCore.Mvc;
using TodoApp.DataAccess.Interface;
using Moq;
using System.Net;
using TodoApp.Api.Test.Base;
using System.Collections.Generic;
using TodoApp.Api.ViewModels;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using TodoApp.DataAccess.Entities;
using TodoApp.Common.Models.Base;

namespace Tests
{
    public class ProjectsControllerTests : BaseTest
    {

        private ProjectsController _projectsController { get; set; }
        private Project _project;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(_sampleUser);

            _project = _sampleUser.Projects.FirstOrDefault();
            userRepositoryMock.Setup(x => x.SaveProject(It.Is<Guid>(y => y == _sampleUser.Id), It.IsAny<Project>())).Returns(new Result() {Data = new Project() });
            userRepositoryMock.Setup(x => x.UpdateProject(It.Is<Guid>(y => y == _sampleUser.Id), It.Is<Project>(y=>y.Id == _project.Id))).Returns(new BaseResult());
            userRepositoryMock.Setup(x => x.UpdateProject(It.Is<Guid>(y => y == _sampleUser.Id), It.Is<Project>(y=>y.Id != _project.Id))).Returns(new BaseResult().AddError("Project Not Found!"));
            userRepositoryMock.Setup(x => x.DeleteProject(It.Is<Guid>(y => y == _sampleUser.Id), It.Is<Guid>(y => y == _project.Id))).Returns(new BaseResult());
            userRepositoryMock.Setup(x => x.DeleteProject(It.Is<Guid>(y => y == _sampleUser.Id), It.Is<Guid>(y => y != _project.Id))).Returns(new BaseResult().AddError("Project Not Found!"));
            
            _projectsController = new ProjectsController(userRepositoryMock.Object, _mapper);
            _projectsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _userClaims }
            };
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.NotFound)]
        public void Test_Get(HttpStatusCode statusCode)
        {
            var projectId = statusCode == HttpStatusCode.OK ? _sampleUser.Projects.FirstOrDefault().Id : Guid.NewGuid();

            var response = _projectsController.Get(projectId) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(statusCode.ToInt());

            if (statusCode == HttpStatusCode.OK)
            {
                var projectVm = (response.Value as Result).Data as ProjectAndTasksViewModel;
                projectVm.Should().NotBeNull();
                projectVm.Id.Should().Be(projectId);
                projectVm.Tasks.Should().NotBeNullOrEmpty();
            }
        }

        [Test]
        [TestCase("Al��veri�", 1)]
        [TestCase("�devler", 0)]
        public void Test_GetAll(string keyword, int count)
        {
            var response = _projectsController.GetAll(keyword) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK.ToInt());
            var projecList = response.Value as List<ProjectViewModel>;
            projecList.Should().NotBeNull();
            projecList.Count.Should().Be(count);
        }

        [Test]
        [TestCase("�devler")]
        public void Test_Post(string projectName)
        {
            var projectVm = new ProjectCreateViewModel { Name = projectName };
            var response = _projectsController.Post(projectVm) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK.ToInt());
            var newProject = (response.Value as Result).Data as ProjectViewModel;
            newProject.Should().NotBeNull();
            newProject.Id.Should().NotBeEmpty();
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Test_Put(HttpStatusCode statusCode)
        {
            var newProjectName = "�devler";
            var project = new ProjectCreateViewModel { Name = newProjectName };
            var projectId = statusCode == HttpStatusCode.OK ? _sampleUser.Projects.FirstOrDefault().Id : Guid.NewGuid();

            var response = _projectsController.Put(projectId, project) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(statusCode.ToInt());
        }

        [Test]
        [TestCase(HttpStatusCode.OK)]
        [TestCase(HttpStatusCode.BadRequest)]
        public void Test_Delete(HttpStatusCode statusCode)
        {
            var projectId = statusCode == HttpStatusCode.OK ? _sampleUser.Projects.FirstOrDefault().Id : Guid.NewGuid();
            var response = _projectsController.Delete(projectId) as ObjectResult;
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(statusCode.ToInt());
        }
    }
}