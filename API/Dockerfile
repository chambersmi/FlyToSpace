FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5050

# Install PowerShell
RUN apt-get update && apt-get install -y wget apt-transport-https software-properties-common \
    && wget -q https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y powershell

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["../API.Application/API.Application.csproj", "../API.Application/"]
COPY ["../API.Domain/API.Domain.csproj", "../API.Domain/"]
COPY ["../API.Infrastructure/API.Infrastructure.csproj", "../API.Infrastructure/"]
RUN dotnet restore "API/API.csproj"

COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

VOLUME ["/data"]
ENV DataDirectory=/data

ENTRYPOINT ["dotnet", "API.dll"]
