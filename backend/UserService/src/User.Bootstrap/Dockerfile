FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY UserService/src/User.Bootstrap/User.Bootstrap.csproj UserService/src/User.Bootstrap/
COPY UserService/src/User.Core.Domain/User.Core.Domain.csproj UserService/src/User.Core.Domain/
COPY UserService/src/User.Core.Application/User.Core.Application.csproj UserService/src/User.Core.Application/
COPY UserService/src/User.Adapters.Inbound.Api/User.Adapters.Inbound.Api.csproj UserService/src/User.Adapters.Inbound.Api/
COPY UserService/src/User.Adapters.Outbound.Postgres/User.Adapters.Outbound.Postgres.csproj UserService/src/User.Adapters.Postgres/
COPY UserService/src/User.Adapters.Outbound.Common/User.Adapters.Outbound.Common.csproj UserService/src/User.Adapters.Outbound.Common/
COPY SharedKernel/SharedKernel.csproj SharedKernel/
COPY AuthTools/AuthTools.csproj AuthTools/ 

# restore
RUN dotnet restore UserService/src/User.Bootstrap/User.Bootstrap.csproj

COPY UserService/src/ UserService/src
COPY SharedKernel/ SharedKernel/
COPY AuthTools/ AuthTools/
COPY Shared.Notifications/ Shared.Notifications/
COPY Shared.RedisStorage/ Shared.RedisStorage/

# build & publish
RUN dotnet publish UserService/src/User.Bootstrap/User.Bootstrap.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "User.Bootstrap.dll"]
