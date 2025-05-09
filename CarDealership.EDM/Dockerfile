FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5220
EXPOSE 7013

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CarDealership.EDM/nuget.config", "./"]
COPY ./nupkgs /nupkgs
COPY ["CarDealership.EDM/CarDealership.EDM.csproj", "CarDealership.EDM/"]
COPY ["CarDealership.EDM.DataAccess/CarDealership.EDM.DataAccess.csproj", "CarDealership.EDM.DataAccess/"]
COPY ["CarDealership.EDM.Core/CarDealership.EDM.Core.csproj", "CarDealership.EDM.Core/"]
RUN dotnet restore "./CarDealership.EDM/CarDealership.EDM.csproj"

COPY . .
WORKDIR "/src/CarDealership.EDM"
RUN dotnet build "./CarDealership.EDM.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarDealership.EDM.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarDealership.EDM.dll"]