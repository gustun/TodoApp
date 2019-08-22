# Prerequests

- You should start and configure couchbase server on your local machine first. 
 You can use this docker command for couchbase installation: 
 ```sh
 docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase
 ```
 # Couchbase Server Configuration
- User name: admin
- Password: 123qwe
- Bucket name: todoApp
- Finally, you should create type index on todoApp bucket
```sh
CREATE INDEX ix_type ON todoApp(type)
```

# Used Libraries
  - Asp.Net Core 2.2 Web API
  - Couchbase Client
  - .NET Standart 2.0
  - Serilog
  - AutoMapper
  - JWT
  - Swagger UI
  - Docker
  - Docker Compose
   
# API Explanation
  - You can use swagger.ui for all of your requests after you run the application.
  - First, you have to login and get an authenticate token. After that use this token in the Authorize section.
  - Now you can try other endpoints. You can crud users, projects and tasks.
  - You can use sample requests for user/register and user/login endpoints.

## Todos

 - Unit tests
 - Automated server configuration
 - Increase request samples
