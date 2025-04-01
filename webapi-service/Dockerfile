# Use official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5163
# EXPOSE 7144

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# COPY webapi-service/webapi-service.csproj webapi-service/

COPY . webapi-service/

RUN ls -la webapi-service/

RUN dotnet restore "webapi-service/webapi-service.csproj"

WORKDIR "/src/webapi-service"
RUN dotnet build "webapi-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "webapi-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapi-service.dll"]
