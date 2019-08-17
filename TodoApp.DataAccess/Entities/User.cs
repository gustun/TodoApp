﻿using TodoApp.DataAccess.Entities.Base;

namespace TodoApp.DataAccess.Entities
{
    public class User: CouchbaseEntity<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
