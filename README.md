# CarBooks

A small motoring-book catalog: browse categories and the books in each category. The solution is a .NET Web API plus a React SPA, orchestrated locally with .NET Aspire.

## Tech stack

### Backend
| Area | Technology |
| --- | --- |
| Runtime / language | .NET 10, C# |
| Web framework | ASP.NET Core |
| Architecture | Layered DDD-style (Domain, Application, Repository, Infrastructure, WebAPI) |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL 18 |
| DI | Autofac |
| API docs | OpenAPI + Swashbuckle |
| Observability | OpenTelemetry (metrics, traces, logs) |
| Log viewer | Seq |
| Orchestration (dev) | .NET Aspire 13 (AppHost, PostgreSQL, Seq, Vite app) |
| Health | ASP.NET Core health checks (`/health`, `/alive`) |

### Frontend
| Area | Technology |
| --- | --- |
| UI | React 19, TypeScript |
| Build | Vite 7 |
| Components | Fluent UI React Components |
| Data fetching | TanStack Query |
| Routing | React Router 8 |
| Node | ≥ 22.22 |

### Packaging & ops
| Area | Technology |
| --- | --- |
| Containers | Docker / Docker Compose (`src/compose`) |
| Images | API (ASP.NET), Web (static SPA + reverse proxy), PostgreSQL, Seq, Aspire Dashboard |

## Solution layout

```
src/
  WebAPI/                 # .NET solution (CarBooks.slnx)
    CarBooks.AppHost/     # Aspire host
    CarBooks.WebAPI/      # HTTP API
    CarBooks.Domain/      # Domain entities & services
    CarBooks.Application/ # Application services & mapping
    CarBooks.Database.Ef/ # EF Core context, migrations, seeding
    CarBooks.Repository/  # EF repository implementations
    ...
  WebApp/                 # React SPA
  compose/                # Production-style docker-compose stack
```

## Running locally

**Aspire (recommended for development)**

```bash
dotnet run --project src/WebAPI/CarBooks.AppHost
```

This starts PostgreSQL, Seq, the API, and the Vite SPA (API URL wired for the Vite proxy).

**Docker Compose**

```bash
cd src/compose
docker compose up --build
```

See `src/compose` for environment variables (`POSTGRES_*`, ports, etc.).

## Tests

**Backend (xUnit)**

```bash
dotnet test src/WebAPI/CarBooks.Domain.Tests/CarBooks.Domain.Tests.csproj
dotnet test src/WebAPI/CarBooks.Application.Tests/CarBooks.Application.Tests.csproj
```

Coverage (Cobertura under `src/WebAPI/TestResults`):

```bash
dotnet test src/WebAPI/CarBooks.slnx \
  --collect:"XPlat Code Coverage" \
  --results-directory src/WebAPI/TestResults \
  --settings src/WebAPI/coverlet.runsettings
```

**Frontend (Vitest)**

```bash
cd src/WebApp
npm test
```

Coverage (text summary + `coverage/index.html`):

```bash
cd src/WebApp
npm run test:coverage
```

CI uploads `backend-coverage` and `frontend-coverage` artifacts, and also sends reports to [Codecov](https://codecov.io) (PR comments + dashboard).

### Codecov setup (one-time)

1. Sign in at [codecov.io](https://codecov.io) with GitHub and add this repository.
2. Copy the upload token from Codecov → repo **Settings**.
3. In GitHub → repo **Settings** → **Secrets and variables** → **Actions**, create secret `CODECOV_TOKEN` with that value.
4. Push to `master` (or open a PR). After the CI run finishes, open the Codecov dashboard or the Codecov bot comment on the PR to browse coverage by file.

Optional README badge (replace `OWNER/REPO`):

```markdown
[![codecov](https://codecov.io/gh/OWNER/REPO/graph/badge.svg)](https://codecov.io/gh/OWNER/REPO)
```
