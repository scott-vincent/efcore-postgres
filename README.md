# efcore-postgres

This is an example of how to write a REST API in ASP.NET Core 2.2 using the Entity Framework (EF) Core.

## Demonstrates the use of:

- ASP.NET Core 2.2
- Entity Framework (EF) Core
- REST
- Postgres
- Swashbuckle (Swagger)
- XUnit
- Moq
- Docker
- Docker Compose

## Description

This example performs basic CRUD operations on a Postgres database and includes generated Swagger documentation.

There is a full set of unit tests written using XUnit and Moq (mocking framework).
These test both the controller and service with full mocking of injected dependencies.

It also includes a Dockerfile and docker-compose file for easy hosting in Docker (where I also hosted Postgres and PgAdmin).
