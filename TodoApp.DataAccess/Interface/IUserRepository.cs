using System;
using TodoApp.Common.Models.Base;
using TodoApp.DataAccess.Entities;

namespace TodoApp.DataAccess.Interface
{
    public interface IUserRepository
    {
        Result Register(User newUser);
        User GetByMail(string mailAddress);
        User GetById(Guid id);
    }
}
