FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# копіюємо всі csproj
COPY Casino.Bootstrap/Casino.Bootstrap.csproj Casino.Bootstrap/
COPY Casino.Core.Domain/Casino.Core.Domain.csproj Casino.Core.Domain/
COPY Casino.Core.Application/Casino.Core.Application.csproj Casino.Core.Application/
COPY Casino.Adapters.Inbound.Api/Casino.Adapters.Inbound.Api.csproj Casino.Adapters.Inbound.Api/
COPY Casino.Adapters.Outbound.Postgres/Casino.Adapters.Outbound.Postgres.csproj Casino.Adapters.Outbound.Postgres/
COPY AuthTools/AuthTools.csproj AuthTools/ 
COPY SharedKernel/SharedKernel.csproj SharedKernel/

# restore
RUN dotnet restore Casino.Bootstrap/Casino.Bootstrap.csproj

# копіюємо весь код
COPY . .

# build & publish
RUN dotnet publish Casino.Bootstrap/Casino.Bootstrap.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Casino.Bootstrap.dll"]
