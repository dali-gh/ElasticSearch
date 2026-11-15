# ElasticSearch Observability API

An ASP.NET Core Web API project exploring application logging and
observability with **Serilog**, **Elasticsearch**, **Kibana**, and
**Docker Compose**.

The goal is to build the project incrementally, understand how
production-oriented logging works, and make application behavior easier
to investigate and troubleshoot.

## Current Features

-   **ASP.NET Core Web API** built with .NET 10.
-   **Structured logging** with Serilog.
-   **Elasticsearch integration** for collecting and storing application
    logs.
-   **Kibana** for searching and inspecting logs.
-   **Exception-handling middleware** for centralized exception
    handling.
-   **Request context enrichment**, including city information in log
    events.
-   **Docker Compose** configuration for the supporting services.

> This README describes the features implemented so far. Planned
> features are listed separately below and are not represented as
> completed.

## Technology Stack

  Technology       Purpose
  ---------------- -----------------------------------
  C# / .NET 10     Web API
  ASP.NET Core     HTTP pipeline and middleware
  Serilog          Structured application logging
  Elasticsearch    Log storage and search
  Kibana           Log exploration and visualization
  Docker Compose   Local service orchestration

## Architecture

``` text
                   HTTP Request
                        |
                        v
              ASP.NET Core Web API
                        |
             Exception-handling middleware
                        |
                        v
                  API Controller
                        |
                        v
                 Serilog Logger
                        |
                        v
                  Elasticsearch
                        |
                        v
                     Kibana
              Search / Explore Logs
```

## Getting Started

### Prerequisites

-   .NET 10 SDK
-   Docker Desktop with Docker Compose
-   Git

### 1. Clone the repository

``` bash
git clone https://github.com/<YOUR_USERNAME>/ElasticSearch.git
cd ElasticSearch
```

### 2. Start the supporting services

``` bash
docker compose up -d
```

Check the services:

``` bash
docker compose ps
```

### 3. Run the API

``` bash
dotnet restore
dotnet run
```

Use the API URL printed by the .NET application. Check
`Properties/launchSettings.json` for the configured local URLs.

### 4. Explore the logs

Open Kibana using the host URL and port configured in
`docker-compose.yml`.

After sending requests to the API, use Kibana Discover to inspect the
log events. The exact Elasticsearch index or data view depends on the
current Serilog sink configuration.

## Example: Inspecting Application Logs

The application uses structured logging so that log properties can be
searched independently instead of being embedded only in plain text
messages.

For example, a log event may contain properties such as:

-   `Level`
-   `MessageTemplate`
-   `City`
-   Exception details, when an exception is logged

The actual fields available in Elasticsearch depend on the configured
Serilog sink and log enrichment.

## Project Structure

``` text
ElasticSearch/
├── Controllers/
│   └── WeatherForecastController.cs
├── Middelware/
│   └── ExceptionHandlingMiddelware.cs
├── Properties/
├── Program.cs
├── WeatherForecast.cs
├── appsettings.json
├── appsettings.Development.json
├── docker-compose.yml
├── ElasticSearch.csproj
└── README.md
```

> The `Middelware` folder name reflects the current project structure
> and can be renamed to `Middleware` as a separate cleanup change.

## Roadmap

The following items are planned improvements:

-   [ ] Add a Correlation ID to identify and connect log events
    belonging to the same HTTP request.
-   [ ] Add request duration logging.
-   [ ] Add health checks for the API and supporting services.
-   [ ] Create Kibana dashboards for errors, request activity, and
    application behavior.
-   [ ] Add OpenTelemetry tracing and metrics.
-   [ ] Add automated tests for middleware and API behavior.
-   [ ] Add a GitHub Actions workflow for build and test validation.

## Security Notes

-   Do not commit passwords, API keys, access tokens, or production
    connection strings.
-   Keep local secrets in user secrets or environment variables.
-   Review configuration files before pushing changes to GitHub.
-   Do not expose Elasticsearch or Kibana publicly without appropriate
    authentication and network security.

## Learning Goals

This project is being developed incrementally to practice:

-   ASP.NET Core middleware and request pipelines
-   Structured logging and log enrichment
-   Centralized log storage and investigation
-   Exception handling
-   Containerized local development
-   Observability concepts used in backend and DevOps engineering

## Status

**In progress.** Features are added and documented as the project
evolves.
