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

        public Result Register(User newUser)
        {
            var result = new Result();
            if (GetByMail(newUser.Email) != null)
                return result.AddError("This email is already being used by another user.");

            result.Data = Create(newUser);
            return result;
        }
    }
}
