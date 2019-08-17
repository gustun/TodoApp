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

        public Result Register(User newUser)
        {
            var result = new Result();
            if (_userList.Any(x => x.Email == newUser.Email))
                return result.AddError("User already exists!");

            newUser.Password = _cryptoHelper.Hash(newUser.Password);
            _userList.Add(newUser);
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
    }
}
