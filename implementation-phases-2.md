## Phase 14 — Land API Controller

In this phase, we created the first API controller for the Land Acquisition module.

The `LandOpportunitiesController` exposes HTTP endpoints for external clients such as Swagger, Angular, or any future frontend application. It currently supports two actions: `GET` to retrieve land opportunities and `POST` to create a new land opportunity.

The controller does not contain business logic. This is an important Clean Architecture principle. The controller only receives the request, sends a command or query through MediatR, and returns the response.

The `GET` endpoint sends `GetLandOpportunitiesQuery`, which reads land opportunities from the Application layer. The `POST` endpoint sends `CreateLandOpportunityCommand`, which creates a new land opportunity.

This phase connects the API layer to the CQRS Application layer. The Land module can now be tested from Swagger.

The goal is to keep the controller thin, readable, and focused only on HTTP concerns.

Developer Notes

The backend foundation is now complete. The next major milestone is security. We will introduce users, roles, permissions, JWT authentication, and authorization so every action in the system can be tied to a real person. This will replace the temporary CurrentUserService implementation and allow audit fields such as CreatedBy and ModifiedBy to contain meaningful values.

After security, we will introduce pagination and filtering. These are not optional features; they are required for scalability. Real estate development platforms can eventually contain thousands of records, so data must be returned in manageable pages and filtered efficiently.

Finally, we will evolve the Land module from simple CRUD operations into a business workflow. Land opportunities will move through a controlled lifecycle, allowing the platform to reflect how real property developers evaluate, acquire, and manage development sites. This transition from data storage to business process management is where the platform begins to deliver real business value.

## Phase 15 — Pagination Foundation

In this phase, we improved the Land Opportunities read workflow by adding pagination.

Before this phase, the query returned all land opportunities. That is acceptable for a tiny test system, but it is not suitable for a real enterprise application. If the database eventually contains thousands of land opportunities, returning every row would be slow and inefficient.

We added paging support by allowing the query to accept `PageNumber` and `PageSize`. The repository now has `GetPagedAsync` and `CountAsync` methods. Infrastructure implements these using EF Core with `Skip`, `Take`, and `CountAsync`.

The query handler now returns a `PagedResult<LandOpportunityDto>` instead of a plain list. This gives the frontend both the current page of data and important metadata such as total records and total pages.

This keeps the API scalable and prepares the Land module for real list screens.

## Architecture Correction — Generic Repository Foundation

We reviewed the Land repository design and noticed that paging, counting, searching, and filtering methods were being added directly to the Land-specific repository. This would work for one module, but it would become repetitive and hard to maintain once we add Planning, Construction, Finance, Sales, Rentals, and other modules.

The corrected design introduces a generic repository for common persistence operations. `IGenericRepository<TEntity>` will contain common methods such as add, get by id, list, count, and paginated retrieval. These behaviours are not specific to Land Acquisition, so they should not be repeated in every module repository.

Specific repositories will still exist, but only for module-specific behaviour. For example, `ILandOpportunityRepository` can inherit from `IGenericRepository<LandOpportunity>` and later add Land-specific methods such as acquisition pipeline summaries or due diligence reports.

This design keeps CQRS intact. Commands and queries still live in the Application layer. The repository only handles persistence abstraction. EF Core remains inside Infrastructure, while the Application layer depends only on repository interfaces.

The key lesson is: reuse common persistence logic through a generic repository, but keep business-specific queries in specific repositories or query services.

Phase 16 — Generic Repository Foundation
Developer Notes

During this phase, we reviewed the architecture and recognised that our repository design was starting to become repetitive. The Land repository contained methods for paging, counting, and searching. While this works for a single module, it becomes difficult to maintain once additional modules such as Planning, Construction, Finance, Sales, Rentals, and Property Management are introduced.

To solve this problem, we introduced the concept of a Generic Repository.

The purpose of the Generic Repository is to centralise common data-access behaviour. Instead of every module implementing methods such as Add, Get By Id, List, Count, and Pagination repeatedly, these capabilities are implemented once and reused throughout the application.

We created a generic interface named IGenericRepository<TEntity>. This interface provides common persistence operations that are applicable to any entity in the system. The Infrastructure layer then provides a generic implementation using Entity Framework Core.

Specific repositories continue to exist. For example, ILandOpportunityRepository still exists because the Land module may eventually require business-specific queries. However, it now inherits from the generic repository and only needs to contain behaviour unique to the Land domain.

This design improves maintainability, reduces duplicated code, and ensures a consistent data access pattern across all modules. It also keeps Entity Framework Core inside the Infrastructure layer, preserving Clean Architecture boundaries.

The key lesson from this phase is that generic functionality should be implemented once and reused everywhere, while business-specific functionality should remain inside module-specific repositories or query services.

# STOP and re-evaluate business need

We have a solution structure with API, Application, Domain, Infrastructure, Shared, and Tests. We added shared models, exception handling, correlation IDs, EF Core, SQL Server LocalDB, a LandOpportunity entity, EF mapping, migration/database support, CQRS command/query handlers, a repository abstraction, a generic repository correction, API controller endpoints, and Swagger testing. Most importantly, the Land Opportunity flow now works end to end: Swagger calls the controller, the controller calls MediatR, MediatR calls the handler, the handler uses the repository, EF Core saves or reads from SQL Server, and the response comes back correctly.

That means the architecture has been proven. We no longer need to prove that Clean Architecture, CQRS, EF Core, Swagger, and the repository boundary can work together. They do work.

Now the logical next big phase should not be adding more random Land CRUD methods. The next big phase should be turning the Land module from a technical CRUD feature into a proper business module.

The biggest missing piece is workflow.

At the moment, a Land Opportunity is basically a record. But in the real business, a land opportunity has a lifecycle. It starts as Identified. Then it moves to Initial Review. Then Due Diligence. Then Offer Made. Then Under Contract. Then Acquired or Rejected. That journey is where the business value lives.

Phase 17 — Land Opportunity Details Query
Developer Notes

In this phase, we implemented the ability to retrieve a single Land Opportunity by its unique identifier.

While this may appear to be a small feature, it is one of the most important capabilities in any enterprise application. Almost every business workflow eventually begins by loading a specific record from the database.

We created a new CQRS query called GetLandOpportunityByIdQuery. This query represents the request to retrieve a single Land Opportunity. The query handler uses the repository abstraction to locate the record and then maps the domain entity into a Data Transfer Object (DTO) that can be returned to the API layer.

A new API endpoint was added:

GET /api/v1/land-opportunities/{id}

This endpoint allows Swagger, Angular, mobile applications, or any future client to request a specific Land Opportunity.

The significance of this phase extends far beyond a simple query. The ability to load a single record is the foundation for many future features, including:

Details screens
Edit screens
Workflow screens
Document management
Approval processes
Audit history
Activity timelines
Reporting views

Without this capability, users could only view lists of records. With it, the application can now support rich business interactions around individual Land Opportunities.

The key lesson from this phase is that enterprise systems are built around business entities. Before users can edit, approve, review, or manage a record, the system must first be able to retrieve that record reliably by its identifier.

## Phase 18 — Land Opportunity Update Foundation

In this phase, we added update support for the Land Acquisition module. This is an important step before workflow because a real business record must be editable before it can move through richer lifecycle rules.

We created `UpdateLandOpportunityCommand`, which represents the user’s intention to edit an existing land opportunity. We added a validator to ensure the id is present and the main fields remain valid.

The command handler retrieves the existing land opportunity using the repository abstraction. If the record does not exist, it throws a `NotFoundException`. If it exists, the handler updates the editable fields, sets audit information such as `UpdatedAtUtc` and `UpdatedBy`, then saves changes through `IUnitOfWork`.

We also added a `PUT` endpoint to the Land Opportunities controller. This endpoint receives the route id and command body, checks that both ids match, sends the command through MediatR, and returns the updated DTO.

This phase keeps the controller thin, keeps EF Core inside Infrastructure, and continues the CQRS pattern.
