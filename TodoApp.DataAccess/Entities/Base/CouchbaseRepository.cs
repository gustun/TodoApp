using Couchbase.Core;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using TodoApp.DataAccess.Interface;

namespace TodoApp.DataAccess.Entities.Base
{
    public abstract class CouchbaseRepository<T>
        where T : CouchbaseEntity<T>
    {
        protected readonly IBucket _bucket;

        public CouchbaseRepository(ITodoBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket();
        }

        public T Get(Guid id)
        {
            var result = _bucket.Get<T>(CreateKey(id));
            if (!result.Success)
                throw result.Exception;

            return result.Value;
        }

        public IEnumerable<T> GetAll(int limit = 10)
        {
            var query = new QueryRequest(
                $@"SELECT t.* 
                FROM {_bucket.Name} as t 
                WHERE type = '{Type}' 
                LIMIT {limit};");

            var result = _bucket.Query<T>(query);
            if (!result.Success)
                throw result.Exception;

            return result.Rows;
        }

        public T Create(T item)
        {
            var result = _bucket.Insert(CreateKey(item.Id), item);
            if (!result.Success)
                throw result.Exception;

            return result.Value;
        }

        public T Update(T item)
        {
            var result = _bucket.Replace(CreateKey(item.Id), item);
            if (!result.Success)
                throw result.Exception;

            return result.Value;
        }

        public void Delete(Guid id)
        {
            var result = _bucket.Remove(CreateKey(id));
            if (!result.Success)
                throw result.Exception;
        }

        public string CreateKey(Guid id) => $"{Type}::{id}";
        public string Type => typeof(T).Name.ToLower();
    }
}
