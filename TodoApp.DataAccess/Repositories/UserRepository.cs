using System;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;
using TodoApp.DataAccess.Interface;

namespace TodoApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetByMail(string mailAddress)
        {
            throw new NotImplementedException();
        }

        public Result Register(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
