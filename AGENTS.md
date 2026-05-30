# AGENTS

## Repository Info

- Name: Quizer API
- Path: `/home/elotoja/Projects/quizer/main`
- Solution: `Quizer.slnx`
- Architecture: CLEAN architecture + DDD
- Main projects:
  - `src/Quizer.Api`
  - `src/Quizer.Infrastructure`
- Tests: `tests/`
- Containerization files: `Dockerfile`, `docker-compose.yml`

## Notes

Database migration commands (from README):

```pwsh
dotnet ef migrations add -p .\src\Quizer.Infrastructure\Quizer.Infrastructure.csproj -s .\src\Quizer.Api\Quizer.Api.csproj InitialSchema
dotnet ef database update -p .\src\Quizer.Infrastructure\Quizer.Infrastructure.csproj -s .\src\Quizer.Api\Quizer.Api.csproj
```
