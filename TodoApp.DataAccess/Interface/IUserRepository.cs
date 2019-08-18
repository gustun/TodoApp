using System;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;

namespace TodoApp.DataAccess.Interface
{
    public interface IUserRepository
    {
        Result Save(User newUser);
        User GetByMail(string mailAddress);
        User GetById(Guid id);

        Result SaveProject(Guid userId, Project newProject);
        BaseResult UpdateProject(Guid userId, Project project);
        BaseResult DeleteProject(Guid userId, Guid projectId);

        Result SaveTask(Guid userId, Guid projectId, ProjectTask newTask);
        BaseResult UpdateTask(Guid userId, Guid projectId, ProjectTask task);
        BaseResult DeleteTask(Guid userId, Guid projectId, Guid taskId);
    }
}
