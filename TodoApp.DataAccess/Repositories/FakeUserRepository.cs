using System;
using System.Collections.Concurrent;
using System.Linq;
using TodoApp.Common.Interface;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;
using TodoApp.DataAccess.Interface;
using TodoApp.Common;

namespace TodoApp.DataAccess.Repositories
{
    public class FakeUserRepository : IUserRepository
    {
        private static ConcurrentBag<User> _userList;
        private ICryptoHelper _cryptoHelper;

        public FakeUserRepository(ICryptoHelper cryptoHelper)
        {
            _cryptoHelper = cryptoHelper;
            if (_userList.IsNullOrEmpty())
                _userList = new ConcurrentBag<User>
                {
                    new User
                    {
                        Email = "gokcan.ustun@yandex.com",
                        Name = "Gokcan",
                        Surname = "Ustun",
                        Password = _cryptoHelper.Hash("1"),
                    }
                };
        }

        public Result Save(User newUser)
        {
            var result = new Result();
            if (_userList.Any(x => x.Email == newUser.Email))
                return result.AddError("This email is already being used by another user.");

            _userList.Add(newUser);
            result.Data = newUser;
            return result;
        }

        public User GetById(Guid id)
        {
            return _userList.FirstOrDefault(x => x.Id == id);
        }

        public User GetByMail(string mailAddress)
        {
            return _userList.FirstOrDefault(x => x.Email == mailAddress);
        }

        public Result SaveProject(Guid userId, Project newProject)
        {
            throw new NotImplementedException();
        }

        public BaseResult UpdateProject(Guid userId, Project project)
        {
            throw new NotImplementedException();
        }

        public BaseResult DeleteProject(Guid userId, Guid projectId)
        {
            throw new NotImplementedException();
        }

        public Result SaveTask(Guid userId, Guid projectId, ProjectTask newTask)
        {
            throw new NotImplementedException();
        }

        public BaseResult UpdateTask(Guid userId, Guid projectId, ProjectTask task)
        {
            throw new NotImplementedException();
        }

        public BaseResult DeleteTask(Guid userId, Guid projectId, Guid taskId)
        {
            throw new NotImplementedException();
        }
    }
}
