# efcore-postgres

This is an example of how to write a REST API in ASP.NET Core 2.2 using the Entity Framework (EF Core).

It performs basic CRUD operations on a Postgres database and includes generated Swagger documentation.
It also includes a Dockerfile and docker-compose file for easy hosting in Docker (where I also hosted Postgres and PgAdmin).

Two sets of unit tests are also included, both using XUnit.
One set tests the controller by replacing the service with a manually-created mocked version.
The other set uses Moq (a mocking framework) to test both the controller and service with full mocking.