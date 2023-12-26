FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Quizer.Api/Quizer.Api.csproj", "Quizer.Api/"]
COPY ["src/Quizer.Contracts/Quizer.Contracts.csproj", "Quizer.Contracts/"]
COPY ["src/Quizer.Application/Quizer.Application.csproj", "Quizer.Application/"]
COPY ["src/Quizer.Domain/Quizer.Domain.csproj", "Quizer.Domain/"]
COPY ["src/Quizer.Infrastructure/Quizer.Infrastructure.csproj", "Quizer.Infrastructure/"]
RUN dotnet restore "Quizer.Api/Quizer.Api.csproj"
COPY . .
WORKDIR "/src/Quizer.Api"
RUN dotnet build "./Quizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quizer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quizer.Api.dll"]