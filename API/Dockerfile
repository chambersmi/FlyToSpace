# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app
EXPOSE 7149

# Create www-data user if it doesn't exist (safe way)
RUN if ! getent group www-data > /dev/null; then \
      addgroup --system www-data; \
    fi && \
    if ! id -u www-data > /dev/null 2>&1; then \
      adduser --system --ingroup www-data www-data; \
    fi

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["API.Application/API.Application.csproj", "API.Application/"]
COPY ["API.Domain/API.Domain.csproj", "API.Domain/"]
COPY ["API.Infrastructure/API.Infrastructure.csproj", "API.Infrastructure/"]

RUN dotnet restore "API/API.csproj"

COPY .. .

WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

RUN mkdir -p /data && chown -R www-data:www-data /data

USER www-data

ENV DATA_DIRECTORY=/data

ENTRYPOINT ["dotnet", "API.dll"]
