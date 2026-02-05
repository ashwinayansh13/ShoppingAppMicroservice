# Quick Start Guide - Microservices

## Prerequisites
- .NET 8 SDK
- Docker Desktop (for Docker Compose)

## Running with Docker Compose (Easiest)

1. **Start all services:**
   ```bash
   docker-compose up --build
   ```

2. **Access the application:**
   - Frontend: http://localhost:5000
   - Gallery API: http://localhost:5001/swagger
   - Checkout API: http://localhost:5002/swagger

3. **Stop services:**
   ```bash
   docker-compose down
   ```

## Running Individual Services

### 1. GalleryService
```bash
cd Services/GalleryService
dotnet run
```
Runs on: http://localhost:5001

### 2. CheckoutService
```bash
cd Services/CheckoutService
dotnet run
```
Runs on: http://localhost:5002

### 3. WebGateway
```bash
cd Services/WebGateway
dotnet run
```
Runs on: http://localhost:5000

**Note:** When running individually, make sure to start GalleryService and CheckoutService before WebGateway.

## Building the Solution

```bash
dotnet build SimpleProject.sln
```

## Testing the APIs

### GalleryService
```bash
# Get all items
curl http://localhost:5001/api/gallery

# Get item by ID
curl http://localhost:5001/api/gallery/1

# Get multiple items
curl -X POST http://localhost:5001/api/gallery/items \
  -H "Content-Type: application/json" \
  -d "[1,2,3]"
```

### CheckoutService
```bash
# Process payment
curl -X POST http://localhost:5002/api/checkout/pay \
  -H "Content-Type: application/json" \
  -d '[{"id":1,"title":"Test","price":49.99}]'
```

## Architecture

- **3 Microservices** total
- **1 Shared Library** (SharedModels)
- **Independent deployment** for each service
- **HTTP REST APIs** for inter-service communication
