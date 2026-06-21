FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["TechSouq.API.sln", "./"]

COPY ["TechSouq-API/*.csproj", "TechSouq-API/"]
COPY ["TechSouq-Business-Layer/*.csproj", "TechSouq-Business-Layer/"]
COPY ["TechSouq.Domian/*.csproj", "TechSouq.Domian/"]
COPY ["TechSouq-DataLayer/*.csproj", "TechSouq-DataLayer/"]
COPY ["TechSouq-Shared-Dtos/*.csproj", "TechSouq-Shared-Dtos/"]
COPY ["TechSouqLogs/*.csproj", "TechSouqLogs/"]

RUN dotnet restore "TechSouq.API.sln"

COPY . .

WORKDIR "/app/TechSouq-API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechSouq.API.dll"]