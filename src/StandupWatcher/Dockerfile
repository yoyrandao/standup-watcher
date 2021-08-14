FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["StandupWatcher.csproj", "StandupWatcher/"]
RUN dotnet restore "StandupWatcher/StandupWatcher.csproj"
WORKDIR "/src/StandupWatcher"
COPY . .
RUN dotnet build "StandupWatcher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StandupWatcher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "StandupWatcher.dll", "--environment=Production" ]