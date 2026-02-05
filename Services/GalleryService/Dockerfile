# Multi-stage Dockerfile for GalleryService (.NET 8.0 ASP.NET Core)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy SharedModels project first (dependency)
COPY ["Shared/SharedModels/SharedModels.csproj", "Shared/SharedModels/"]

# Copy GalleryService project file
COPY ["Services/GalleryService/GalleryService.csproj", "Services/GalleryService/"]

# Restore dependencies for SharedModels
WORKDIR /src/Shared/SharedModels
RUN dotnet restore

# Restore dependencies for GalleryService
WORKDIR /src/Services/GalleryService
RUN dotnet restore

# Copy all source files
COPY ["Shared/SharedModels/", "Shared/SharedModels/"]
COPY ["Services/GalleryService/", "Services/GalleryService/"]

# Build and publish GalleryService
WORKDIR /src/Services/GalleryService
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published files from build stage
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 80
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

# Set entry point
ENTRYPOINT ["dotnet", "GalleryService.dll"]
