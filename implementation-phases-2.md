## Phase 14 — Land API Controller

In this phase, we created the first API controller for the Land Acquisition module.

The `LandOpportunitiesController` exposes HTTP endpoints for external clients such as Swagger, Angular, or any future frontend application. It currently supports two actions: `GET` to retrieve land opportunities and `POST` to create a new land opportunity.

The controller does not contain business logic. This is an important Clean Architecture principle. The controller only receives the request, sends a command or query through MediatR, and returns the response.

The `GET` endpoint sends `GetLandOpportunitiesQuery`, which reads land opportunities from the Application layer. The `POST` endpoint sends `CreateLandOpportunityCommand`, which creates a new land opportunity.

This phase connects the API layer to the CQRS Application layer. The Land module can now be tested from Swagger.

The goal is to keep the controller thin, readable, and focused only on HTTP concerns.
