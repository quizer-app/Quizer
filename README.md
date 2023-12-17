# Quizer API

## Description

This project implements CLEAN architecture and DDD in a simple quiz application.

## Migration

To run a database migration, run the following command:

```pwsh
dotnet ef migrations add -p .\src\Quizer.Infrastructure\Quizer.Infrastructure.csproj -s .\src\Quizer.Api\Quizer.Api.csproj MIGRATION_NAME
dotnet ef database update -p .\src\Quizer.Infrastructure\Quizer.Infrastructure.csproj -s .\src\Quizer.Api\Quizer.Api.csproj
```
