FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY AuthService/src/Auth.Bootstrap/Auth.Bootstrap.csproj AuthService/src/Auth.Bootstrap/
COPY AuthService/src/Auth.Core.Domain/Auth.Core.Domain.csproj AuthService/src/Auth.Core.Domain/
COPY AuthService/src/Auth.Core.Application/Auth.Core.Application.csproj AuthService/src/Auth.Core.Application/
COPY AuthService/src/Auth.Adapters.Inbound.Api/Auth.Adapters.Inbound.Api.csproj AuthService/src/Auth.Adapters.Inbound.Api/
COPY AuthService/src/Auth.Adapters.Outbound.PostgresEfWriteAccess/Auth.Adapters.Outbound.PostgresEfWriteAccess.csproj AuthService/src/Auth.Adapters.PostgresEfWriteAccess.Outbox/
COPY AuthService/src/Auth.Adapters.Outbound.Common/Auth.Adapters.Outbound.Common.csproj AuthService/src/Auth.Adapters.Outbound.Common/
COPY SharedKernel/SharedKernel.csproj SharedKernel/
COPY Shared.RedisStorage/Shared.RedisStorage.csproj Shared.RedisStorage/
COPY Shared.Notifications/Shared.Notifications.csproj Shared.Notifications/
COPY AuthTools/AuthTools.csproj AuthTools/ 

# restore
RUN dotnet restore AuthService/src/Auth.Bootstrap/Auth.Bootstrap.csproj

COPY AuthService/src/ AuthService/src
COPY SharedKernel/ SharedKernel/
COPY AuthTools/ AuthTools/
COPY Shared.Notifications/ Shared.Notifications/
COPY Shared.RedisStorage/ Shared.RedisStorage/

# build & publish
RUN dotnet publish AuthService/src/Auth.Bootstrap/Auth.Bootstrap.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Auth.Bootstrap.dll"]
