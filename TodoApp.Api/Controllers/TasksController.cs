using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TodoApp.Api.Infrastructure;
using TodoApp.DataAccess.Interface;
using TodoApp.DataAccess.Entities;
using TodoApp.Api.ViewModels;
using System.Collections.Generic;
using TodoApp.Api.ViewModels.Requests;
using TodoApp.Common;
using TodoApp.Common.Models.Base;

namespace TodoApp.Api.Controllers
{
    [ApiController, Route("v1/projects/{projectId}/tasks")]
    public class TasksController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TasksController(IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(Guid projectId, [FromQuery] GetTasksRequest filterOptions)
        {
            var user = _userRepository.GetById(GetUserId());
            if (user == null)
                return NotFound("User not found.");

            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(projectId));
            if (project == null)
                return NotFound("Project not found.");

            var response = ApplyFilters(filterOptions, project.Tasks);
            return Ok(response);
        }

        private PagedResult<ProjectTaskViewModel> ApplyFilters(GetTasksRequest filterOptions, IEnumerable<ProjectTask> taskList)
        {
            var pagedResult = new PagedResult<ProjectTaskViewModel>() { CurrentPage = filterOptions.PageIndex, PageSize = filterOptions.PageSize, RowCount = 0 };
            if (taskList.IsNullOrEmpty())
                return pagedResult;

            if (filterOptions.IsCompleted.HasValue)
                taskList = taskList.Where(x => x.IsCompleted == filterOptions.IsCompleted.Value);

            if (filterOptions.IsImportant.HasValue)
                taskList = taskList.Where(x => x.IsImportant == filterOptions.IsImportant.Value);

            if (filterOptions.DeadlineRange.Min.HasValue)
                taskList = taskList.Where(x => x.Deadline >= filterOptions.DeadlineRange.Min.Value);

            if (filterOptions.DeadlineRange.Max.HasValue)
                taskList = taskList.Where(x => x.Deadline <= filterOptions.DeadlineRange.Max.Value);

            pagedResult.RowCount = taskList.Count();
            pagedResult.PageCount = (int)Math.Ceiling((double)pagedResult.RowCount / pagedResult.PageSize);
            taskList = taskList
                .Skip((filterOptions.PageIndex - 1) * filterOptions.PageSize)
                .Take(filterOptions.PageSize).ToList();
            pagedResult.Results = _mapper.Map<List<ProjectTaskViewModel>>(taskList);
            return pagedResult;
        }

        [HttpPost]
        public IActionResult Post(Guid projectId, ProjectTaskCreateViewModel vm)
        {
            var result = _userRepository.SaveTask(GetUserId(), projectId, _mapper.Map<ProjectTask>(vm));

            if (!result.IsSuccess)
                return BadRequest(result);

            result.Data = _mapper.Map<ProjectTaskViewModel>(result.Data);
            return Ok(result);
        }

        [HttpPatch("{taskId}")]
        public IActionResult Patch(Guid projectId, Guid taskId, ProjectTaskCreateViewModel vm)
        {
            var task = _mapper.Map<ProjectTask>(vm);
            task.Id = taskId;
            var result = _userRepository.UpdateTask(GetUserId(), projectId, task);

            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        public IActionResult Delete(Guid projectId, Guid taskId)
        {
            var result = _userRepository.DeleteTask(GetUserId(), projectId, taskId);

            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }
    }
}
