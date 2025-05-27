FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["VehicleService.sln", "./"]
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

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "VehicleService.API.dll"]