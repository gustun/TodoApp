using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Api.Infrastructure;
using TodoApp.Api.ViewModels;
using TodoApp.DataAccess.Entities;
using TodoApp.DataAccess.Interface;

namespace TodoApp.Api.Controllers
{
    [ApiController, Route("v1/projects")]
    public class ProjectsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ProjectsController(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(string searchTerm = "")
        {
            var user = _userRepository.GetById(GetUserId());
            if (user == null)
                return NotFound("User not found.");

            searchTerm = searchTerm.Trim();
            var projects = user.Projects;
            if (!string.IsNullOrEmpty(searchTerm))
                projects = projects.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

            var response = _mapper.Map<List<ProjectSimpleResponse>>(projects);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Post(ProjectSimpleViewModel newProject)
        {
            var result = _userRepository.SaveProject(GetUserId(), _mapper.Map<Project>(newProject));

            if (!result.IsSuccess)
                return BadRequest(result);

            result.Data = _mapper.Map<ProjectSimpleResponse>(result.Data);
            return Ok(result);
        }

        [HttpPatch("{projectId}")]
        public IActionResult Patch(Guid projectId, ProjectSimpleViewModel project)
        {
            var projectEntity = _mapper.Map<Project>(project);
            projectEntity.Id = projectId;
            var result = _userRepository.UpdateProject(GetUserId(), projectEntity);

            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{projectId}")]
        public IActionResult Delete(Guid projectId)
        {
            var result = _userRepository.DeleteProject(GetUserId(), projectId);

            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
