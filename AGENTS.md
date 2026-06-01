# AGENTS

## Repository Info

- Name: Quizer
- Path: `/home/elotoja/Projects/quizer/main`
- Solution: `Quizer.slnx`
- Architecture: CLEAN architecture + DDD
- Backend projects:
  - `src/Quizer.Domain`
  - `src/Quizer.Application`
  - `src/Quizer.Infrastructure`
  - `src/Quizer.Api`
  - `src/Quizer.Contracts`
- Frontend project:
  - `frontend/` (Vite + React + TypeScript)
- Tests: `tests/`
- Containerization files: `Dockerfile`, `docker-compose.yml`

## Development Environment

- Preferred shell: `nix develop`
- Toolchain:
  - .NET SDK 10
  - Node.js 22
  - pnpm 11
- Devshell helper commands:
  - `nix develop -c restore`
  - `nix develop -c api`
  - `nix develop -c web`

## Notes

Database migration commands:

```bash
dotnet ef migrations add -p ./src/Quizer.Infrastructure/Quizer.Infrastructure.csproj -s ./src/Quizer.Api/Quizer.Api.csproj <MigrationName>
dotnet ef database update -p ./src/Quizer.Infrastructure/Quizer.Infrastructure.csproj -s ./src/Quizer.Api/Quizer.Api.csproj
```