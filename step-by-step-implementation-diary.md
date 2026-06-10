## Phase 0 — What We Are Building and Why

We are building BuildEstate Pro, a real estate development lifecycle platform. The purpose of the application is to help a property development company manage the full journey of a development project, starting from identifying a land opportunity and later moving through planning, legal work, construction, sales, handover, and long-term operations.

For now, we are not building the full system. We are starting slowly with the backend architecture and then the Land Acquisition module. The Land module is the correct first module because every development begins with land. Before a company can build, sell, rent, or manage properties, it must first find land, evaluate the opportunity, perform due diligence, and decide whether to acquire it.

The application will use Clean Architecture. This means the code will be separated into clear projects. Domain will contain business objects. Application will contain use cases. Infrastructure will handle database and technical services. API will expose HTTP endpoints. Shared will contain common response models. Tests will help us verify behaviour.

The goal of this phase is understanding, not speed. We will build a small number of classes at a time, run the build often, and document every step so the architecture becomes clear and manageable.

## Phase 1 — Create the Solution Structure

In this phase, we created the empty backend solution structure for BuildEstate Pro. We did not add business logic yet. We only created the main projects that will hold the application architecture.

The solution contains six important parts. BuildEstate.API will be the web API project that receives HTTP requests from the browser. BuildEstate.Domain will contain the core business objects, such as future Land Opportunity entities. BuildEstate.Application will contain business use cases, commands, queries, and validation. BuildEstate.Infrastructure will contain database access and technical implementations. BuildEstate.Shared will contain common response models and helpers used across projects. BuildEstate.Application.Tests will contain automated tests for the application layer.

This structure supports Clean Architecture. The main idea is to keep business rules separate from technical details. We do not want controllers, database code, and business logic mixed together in one large project. By separating the projects now, the application will be easier to understand, test, and extend later.

The goal of this phase is simple: create the solution, add the projects, add them to the solution file, and confirm that everything builds successfully.

## Phase 2 — Add Project References

In this phase, we connected the projects together using project references. This is an important Clean Architecture step because references control the direction of dependencies.

The Domain project remains independent and does not reference any other project. This keeps the business model clean. The Application project references Domain because application use cases need to work with business entities. Infrastructure references Application and Domain because it provides technical implementations such as database access and external services. API references Application and Infrastructure because the API receives HTTP requests and starts the application pipeline.

This structure protects the core business logic. We do not want the Domain project to know about ASP.NET Core, Entity Framework, SQL Server, Swagger, or Identity. Those technologies live outside the business model.

The key rule is simple: dependencies should point inward. Outer projects can depend on inner projects, but inner projects should not depend on outer projects.

After adding the references, we ran `dotnet build` to confirm that the solution still compiles successfully.

## Phase 3 — Add Minimal NuGet Packages

In this phase, we added only the NuGet packages needed for the early backend foundation. We deliberately avoided adding too many packages at once because that can confuse the architecture and make errors harder to diagnose.

The Application project received MediatR and FluentValidation. MediatR will help us organise business use cases as commands and queries. FluentValidation will help us validate user input in a clean and testable way.

The Infrastructure project received Entity Framework Core, the SQL Server provider, and EF Core Design. These packages allow the application to connect to SQL Server and later create database migrations.

The API project received Swagger support so we can test endpoints from the browser once we create controllers.

The test project received FluentAssertions and Moq. These will help us write clear tests for the application layer.

We did not add Identity, JWT, email, file storage, or AutoMapper yet. Those will come later when the developer understands the basic architecture flow.

## Phase 4 — First Shared Foundation Classes

In this phase, we created the first reusable foundation classes for the backend architecture. These classes are small, but they will be used across many future modules.

The `BaseEntity` class lives in the Domain project. It gives future business entities a common structure, including an Id, created date, created by, updated date, updated by, and soft-delete flag. This means future entities such as `LandOpportunity` do not need to repeat these same fields again and again.

The `ApiResponse<T>` class lives in the Shared project. It gives the API a consistent response shape, so endpoints can return success, message, data, and errors in a predictable way.

## Phase 5 — Application Abstractions and Dependency Injection

In this phase, we added the first Application-layer abstractions. These interfaces allow the Application project to describe what it needs without depending on technical details.

`ICurrentUserService` represents the currently logged-in user. The Application layer can use this later to record who created or updated a record, but it does not need to know anything about HTTP, JWT, or ASP.NET Core.

`IDateTimeProvider` gives the application a testable way to access the current UTC time. This is better than calling `DateTime.UtcNow` everywhere because tests can later control the time if needed.

`IUnitOfWork` gives the Application layer a simple abstraction for saving changes. Infrastructure will later implement this using Entity Framework Core.

We also created an `AddApplication()` dependency injection method. This registers MediatR and FluentValidation from the Application assembly. Later, the API project will call this method in `Program.cs`.

The goal of this phase is to keep the Application layer clean, testable, and independent from Infrastructure and API details.


The `PagedResult<T>` class prepares us for list screens. In real business applications, list pages may contain hundreds or thousands of records, so we need a standard way to return paginated data.

We also created basic exception classes for not found, bad request, and forbidden access scenarios. These will later be used by middleware to return clean API error responses.

The goal of this phase was to create simple reusable building blocks and confirm the solution still builds successfully.

## Developer Note — Current User, Roles, and Permissions

At this stage, `ICurrentUserService` only tracks basic current-user information: user id, username, email, and whether the request is authenticated. This is enough for early audit support, such as recording who created or updated a record.

Roles and permissions are not added yet because we have not implemented authentication, JWT tokens, users, roles, or permission seeding. Adding them too early would confuse the architecture.

Later, when authentication is introduced, the API will read the logged-in user from `HttpContext.User`. The JWT token will contain claims such as user id, email, name, and roles. The API implementation of `ICurrentUserService` will read those claims and expose them to the Application layer.

Permissions should be handled separately through an `IPermissionService`. A user may have a role such as `AcquisitionManager`, and that role may have permissions such as `Land.View`, `Land.Create`, and `Land.Edit`.

The key design principle is separation of responsibility. `ICurrentUserService` tells us who the user is. `IPermissionService` tells us what the user is allowed to do.


## Phase 6 — Infrastructure Foundation

In this phase, we created the first Infrastructure-layer implementation classes. Infrastructure is the project responsible for technical details such as database access, file storage, email, external services, and other implementation concerns.

We created `ApplicationDbContext`, which inherits from Entity Framework Core `DbContext` and implements `IUnitOfWork`. At this point, the context does not contain any business tables because we have not created the Land module yet. That is expected. The goal is only to prepare the database foundation.

We also created `DateTimeProvider`, which implements `IDateTimeProvider`. This shows the Clean Architecture pattern clearly: the Application layer defines what it needs through an interface, and Infrastructure provides the real implementation.

Finally, we created `AddInfrastructure()`, a dependency injection extension method. Later, the API project will call this method during startup to register EF Core, SQL Server, Unit of Work, and Infrastructure services.

The important lesson is that Infrastructure depends on Application, but Application does not depend on Infrastructure. This keeps business use cases clean and testable.

## Phase 7 — API Startup Wiring

In this phase, we connected the API project to the rest of the backend architecture. The API project is the entry point of the application. When the backend starts, `Program.cs` decides which services are registered and which middleware is active.

We registered controllers so the API can expose HTTP endpoints. We registered Swagger so developers can test endpoints from the browser. We called `AddApplication()` to register MediatR and FluentValidation from the Application layer. We called `AddInfrastructure()` to register Entity Framework Core, SQL Server, Unit of Work, and technical services from the Infrastructure layer.

We also added a connection string in `appsettings.json`. This tells Infrastructure where the SQL Server database will be created later. At this stage, no database tables exist yet because we have not created any business entities.

Finally, we added a health check endpoint at `/health`. This gives us a simple way to confirm that the API is running.

The goal of this phase is to prove that the API can start, load all architecture layers, show Swagger, and respond to a basic health check.

## Phase 8 — Global Exception Middleware and Correlation ID

In this phase, we added two small but important API middleware components.

The first is `GlobalExceptionMiddleware`. Its job is to catch unhandled exceptions in one central place and return a consistent JSON response to the client. Without this middleware, different parts of the API may return different error formats. A consistent error response makes the API easier to test, debug, and consume from the frontend.

The second is `CorrelationIdMiddleware`. Its job is to attach a unique tracking ID to each request. If the client sends an `X-Correlation-ID` header, the API keeps it. If not, the API creates one. The same ID is returned in the response header. Later, this helps developers trace problems through logs.

We registered both middleware classes in `Program.cs` immediately after `builder.Build()`. This means they run early in the request pipeline.

The goal of this phase is to make the API more professional before adding business features. Even simple modules should sit inside clean error handling and request tracing.

## Phase 9 — First Land Domain Model

In this phase, we created the first real business model for the BuildEstate Pro application: `LandOpportunity`.

A land opportunity represents a possible piece of land that the company may want to buy and develop. At this stage, it is not yet a development project. It is simply an opportunity that needs to be reviewed, checked, valued, and possibly acquired.

We also created `LandOpportunityStatus`, an enum that describes the lifecycle of the opportunity. The opportunity may start as `Identified`, move into `InitialReview`, then `DueDiligence`, then `OfferMade`, then `UnderContract`, and finally become either `Acquired` or `Rejected`.

This class belongs in the Domain project because it is a business concept. It does not depend on Entity Framework, ASP.NET Core, controllers, SQL Server, or Swagger. It simply describes something important in the business.

The entity inherits from `BaseEntity`, so it automatically has an Id, audit fields, and soft-delete support. This keeps future entities consistent and avoids repeating common fields.

The goal of this phase is to introduce the first business object slowly and clearly before connecting it to the database or API.


## Phase 10 — Connect LandOpportunity to EF Core

In this phase, we connected the `LandOpportunity` domain entity to Entity Framework Core. Before this phase, `LandOpportunity` was only a C# class in the Domain project. Now Infrastructure knows that this entity should be mapped to a database table.

We added `DbSet<LandOpportunity>` to `ApplicationDbContext`. A `DbSet` represents a table-like collection that EF Core can query and save.

We also created `LandOpportunityConfiguration`. This configuration tells EF Core how to map the entity to SQL Server. It defines the table name, primary key, required fields, maximum string lengths, decimal precision for land size and asking price, and enum storage for the status.

We used `HasConversion<string>()` for the status so the database stores readable values such as `Identified` instead of only numbers. We also added a query filter for soft delete, meaning records marked as deleted will not normally appear in queries.

The goal of this phase is to connect the business entity to database mapping safely before creating migrations or API endpoints.

