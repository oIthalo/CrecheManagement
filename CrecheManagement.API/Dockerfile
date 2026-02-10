FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY CrecheManagement.slnx .
COPY CrecheManagement.API/CrecheManagement.API.csproj CrecheManagement.API/
COPY CrecheManagement.Domain/CrecheManagement.Domain.csproj CrecheManagement.Domain/
COPY CrecheManagement.Infrastructure/CrecheManagement.Infrastructure.csproj CrecheManagement.Infrastructure/

COPY . .

WORKDIR /app/CrecheManagement.API
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "CrecheManagement.API.dll"]