# squares-api-task
Prerequisites

To build, run, and test this project, the following tools and dependencies are required.

Task done on .NET SDK 8.0

Required NuGet Packages:
Microsoft.AspNetCore.Mvc.Core
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.InMemory
Microsoft.NET.Test.Sdk
xUnit
xunit.runner.visualstudio
Moq
Swashbuckle.AspNetCore
Sentry.AspNetCore
Sentry.OpenTelemetry

Sentry configuration DSN should be provided via appsettings.json or environment variables.

# Features

user can import a list of points
user can add a point to an existing list
user can delete a point from an existing list
user can retrieve the squares identified
Automated tests
Containerization
Measuring SLI (Sentry)

# How to run:
1. Debug on Visual studio to access swagger documentation (IMPORTANT sentry dsn key is needed to setup through user secrets or appsettings.json. Can be found in compose.yaml).
2. Run on docker locally: docker compose up --build. It will be availalble on http://localhost:8080/swagger/index.html
