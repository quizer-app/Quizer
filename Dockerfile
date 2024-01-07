FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "./src/Quizer.Api/Quizer.Api.csproj"
RUN dotnet publish "./src/Quizer.Api/Quizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Quizer.Api.dll"]