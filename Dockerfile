FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /Rezerv

COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
# COPY ["MyanmarGoalBackend.sln", "./"]
COPY ["RezervBackend.sln", "./"]

COPY ["WebApi/WebApi.csproj", "./WebApi/"]
COPY ["src/Core/Core.csproj", "./src/Core/"]
COPY ["src/Domain/Domain.csproj", "./src/Domain/"]
COPY ["src/Contract/Contract.csproj", "./src/Contract/"]
COPY ["src/Application/Application.csproj", "./src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "./src/Infrastructure/"]
COPY ["UnitTests/UnitTests.csproj", "./UnitTests/"]
COPY ["src/Persistence/Persistence.csproj", "./src/Persistence/"]
COPY ["src/Presentation/Presentation.csproj", "./src/Presentation/"]

RUN dotnet restore "./RezervBackend.sln"

COPY . ./

FROM build AS publish

RUN dotnet tool install --global dotnet-ef --version 9.0.20
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet publish "./WebApi/WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

ENV ASPNETCORE_URLS=http://0.0.0.0:3000
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 3000

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WebApi.dll"]