# ---------- Build stage ----------
  FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
  WORKDIR /src
  
  # Copy project files first (for layer caching)
  COPY Shared/SharedModels/SharedModels.csproj Shared/SharedModels/
  COPY Services/GalleryService/GalleryService.csproj Services/GalleryService/
  
  # Restore dependencies ONCE (project graph)
  RUN dotnet restore Services/GalleryService/GalleryService.csproj
  
  # Copy the rest of the source
  COPY . .
  
  # Build and publish GalleryService
  RUN dotnet publish Services/GalleryService/GalleryService.csproj \
      -c Release \
      -o /app/publish \
      --no-restore
  
  # ---------- Runtime stage ----------
  FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
  WORKDIR /app
  
  # Install curl for health checks
  RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
  
  # Copy published output
  COPY --from=build /app/publish .
  
  ENV ASPNETCORE_URLS=http://+:80
  ENV ASPNETCORE_ENVIRONMENT=Production
  
  EXPOSE 80
  
  HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost/health || exit 1
  
  ENTRYPOINT ["dotnet", "GalleryService.dll"]
  