using System;

namespace TodoApp.DataAccess.Entities.Base
{
    public abstract class CouchbaseEntity<T>
    {
        private Guid _id { get; set; }
        public Guid Id
        {
            get
            {
                if (_id == null || _id == Guid.Empty)
                    _id = Guid.NewGuid();
                return _id;
            }
            set => _id = value;
        }
        public string Type => typeof(T).Name.ToLower();
    }
}
