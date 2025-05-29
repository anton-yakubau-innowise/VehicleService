FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 8080

COPY ["VehicleService.sln", "VehicleService.sln"]
COPY ["VehicleService.API/VehicleService.API.csproj", "VehicleService.API/"]
COPY ["VehicleService.Application/VehicleService.Application.csproj", "VehicleService.Application/"]
COPY ["VehicleService.Domain/VehicleService.Domain.csproj", "VehicleService.Domain/"]
COPY ["VehicleService.Infrastructure/VehicleService.Infrastructure.csproj", "VehicleService.Infrastructure/"]

RUN dotnet restore "VehicleService.sln"

COPY . .

WORKDIR "/src/VehicleService.API"
RUN dotnet publish "VehicleService.API.csproj" -c Release -o /app/publish --no-restore





FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /src/VehicleService.API/app/publish .

ENTRYPOINT ["dotnet", "VehicleService.API.dll"]