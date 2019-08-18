using Couchbase.N1QL;
using System;
using System.Linq;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;
using TodoApp.DataAccess.Entities.Base;
using TodoApp.DataAccess.Interface;

namespace TodoApp.DataAccess.Repositories
{
    public class UserRepository : CouchbaseRepository<User>, IUserRepository
    {
        public UserRepository(ITodoBucketProvider bucketProvider) 
            : base(bucketProvider)
        {
        }

        public User GetById(Guid id)
        {
            return Get(id);
        }

        public User GetByMail(string mailAddress)
        {
            var query = new QueryRequest(
                $@"SELECT t.* 
                FROM {_bucket.Name} as t 
                WHERE type = '{Type}' AND email = '{mailAddress}'
                LIMIT 1;");

            var result = _bucket.Query<User>(query);
            if (!result.Success)
                throw result.Exception;

            return result.Rows.ToList().FirstOrDefault();
        }

        public Result Save(User newUser)
        {
            var result = new Result();
            if (GetByMail(newUser.Email) != null)
                return result.AddError("This email is already being used by another user.");

            Create(newUser);
            result.Data = newUser;
            return result;
        }

        //projects
        public Result SaveProject(Guid userId, Project newProject)
        {
            var user = Get(userId);
            user.Projects.Add(newProject);
            Update(user);
            return new Result(newProject);
        }

        public BaseResult UpdateProject(Guid userId, Project project)
        {
            var toReturn = new BaseResult();
            var user = Get(userId);
            var index = user.Projects.FindIndex(x=>x.Id.Equals(project.Id));
            if (index == -1)
                return toReturn.AddError("Project Not Found!");

            user.Projects[index] = project;
            Update(user);
            return toReturn.AddSuccess();
        }

        public BaseResult DeleteProject(Guid userId, Guid projectId)
        {
            var toReturn = new BaseResult();
            var user = Get(userId);
            var index = user.Projects.FindIndex(x => x.Id.Equals(projectId));
            if (index == -1)
                return toReturn.AddError("Project Not Found!");

            user.Projects.RemoveAt(index);
            Update(user);
            return toReturn.AddSuccess();
        }

        //tasks
        public Result SaveTask(Guid userId, Guid projectId, ProjectTask newTask)
        {
            var user = Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(projectId));
            if (project == null)
                return new Result().AddError("Project Not Found!");
            project.Tasks.Add(newTask);
            Update(user);
            return new Result(newTask);
        }

        public BaseResult UpdateTask(Guid userId, Guid projectId, ProjectTask task)
        {
            var toReturn = new BaseResult();
            var user = Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(projectId));
            if (project == null)
                return toReturn.AddError("Project Not Found!");

            var index = project.Tasks.FindIndex(x => x.Id.Equals(task.Id));
            if (index == -1)
                return toReturn.AddError("Task Not Found!");

            project.Tasks[index] = task;
            Update(user);
            return toReturn.AddSuccess();
        }

        public BaseResult DeleteTask(Guid userId, Guid projectId, Guid taskId)
        {
            var toReturn = new BaseResult();
            var user = Get(userId);
            var project = user.Projects.FirstOrDefault(x => x.Id.Equals(projectId));
            if (project == null)
                return toReturn.AddError("Project Not Found!");

            var index = project.Tasks.FindIndex(x => x.Id.Equals(taskId));
            if (index == -1)
                return toReturn.AddError("Task Not Found!");

            project.Tasks.RemoveAt(index);
            Update(user);
            return toReturn.AddSuccess();
        }
    }
}
