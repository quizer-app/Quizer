FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN chmod +x ./scripts/move_image_files.sh
RUN ./scripts/move_image_files.sh
RUN dotnet restore "./src/Quizer.Api/Quizer.Api.csproj"
RUN dotnet publish "./src/Quizer.Api/Quizer.Api.csproj" -c $BUILD_CONFIGURATION -o /application

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app

VOLUME /app/logs
VOLUME /app/wwwroot

WORKDIR /application
EXPOSE 8080
EXPOSE 8081
COPY --from=build /application .
COPY --from=build /files .

ENTRYPOINT ["dotnet", "Quizer.Api.dll"]