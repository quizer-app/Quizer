FROM node:22-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package.json frontend/pnpm-lock.yaml ./
RUN corepack enable && pnpm install --frozen-lockfile
COPY frontend .
RUN pnpm build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY . .
COPY --from=frontend-build /frontend/dist ./src/Quizer.Api/wwwroot
RUN dotnet restore "./src/Quizer.Api/Quizer.Api.csproj"
RUN dotnet publish "./src/Quizer.Api/Quizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
ARG ASPNETCORE_URLS=http://+:5000
ARG ASPNETCORE_HTTP_PORTS=5000

EXPOSE 5000

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Quizer.Api.dll"]