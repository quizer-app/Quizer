# Quizer

Quizer is a full-stack quiz platform with a .NET 10 backend and a React/TypeScript frontend.

## Quick start

```bash
# 1) Enter dev shell
nix develop

# 2) Restore backend/frontend dependencies
nix develop -c restore

# 3) Run API (terminal A)
nix develop -c api

# 4) Run web app (terminal B)
nix develop -c web
```

## Project requirements

Use the Nix dev shell to guarantee toolchain parity:

```bash
nix develop
```

This repository expects:

- .NET SDK 10 (`dotnet`)
- Node.js 22 (`node`)
- pnpm 11 (`pnpm`)

The provided `flake.nix` installs and pins these versions.

## Architecture

The backend follows CLEAN architecture with DDD boundaries:

- `src/Quizer.Domain`: aggregates, value objects, domain events, and invariants.
- `src/Quizer.Application`: use cases (commands/queries), handlers, and application contracts.
- `src/Quizer.Infrastructure`: persistence, authentication, external service implementations, EF Core setup.
- `src/Quizer.Api`: HTTP surface (controllers, API wiring, versioning, composition root).
- `src/Quizer.Contracts`: request/response contracts shared at the API boundary.

The frontend lives in `frontend/` and is a Vite + React application that consumes the API.

## Repository layout

- `src/`: backend code
- `tests/`: backend unit/integration tests
- `frontend/`: web client
- `Dockerfile` and `docker-compose.yml`: containerized runtime
- `Quizer.slnx`: backend solution entrypoint

## Local development

Enter the shell and restore dependencies:

```bash
nix develop -c restore
```

Run backend API:

```bash
nix develop -c api
```

Run frontend dev server:

```bash
nix develop -c web
```

## Database migrations

Create a migration:

```bash
dotnet ef migrations add -p ./src/Quizer.Infrastructure/Quizer.Infrastructure.csproj -s ./src/Quizer.Api/Quizer.Api.csproj <MigrationName>
```

Apply migrations:

```bash
dotnet ef database update -p ./src/Quizer.Infrastructure/Quizer.Infrastructure.csproj -s ./src/Quizer.Api/Quizer.Api.csproj
```