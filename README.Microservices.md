# Microservices Architecture

This project has been converted from a monolithic application to a microservices architecture.

## Architecture Overview

The application consists of **3 microservices**:

1. **GalleryService** (Port 5001)
   - REST API for managing gallery items
   - Endpoints: `/api/gallery`
   - Swagger UI: `http://localhost:5001/swagger`

2. **CheckoutService** (Port 5002)
   - REST API for processing payments and orders
   - Endpoints: `/api/checkout`
   - Swagger UI: `http://localhost:5002/swagger`

3. **WebGateway** (Port 5000)
   - MVC frontend application
   - Acts as API Gateway, calling other microservices
   - Main entry point: `http://localhost:5000`

## Project Structure

```
SimpleProject/
├── Shared/
│   └── SharedModels/          # Shared DTOs/models
├── Services/
│   ├── GalleryService/         # Gallery microservice
│   ├── CheckoutService/        # Checkout microservice
│   └── WebGateway/            # Frontend gateway
├── docker-compose.yml          # Docker Compose for local development
└── azure-pipelines.yml        # CI/CD pipeline
```

## Running Locally

### Option 1: Using Docker Compose (Recommended)

```bash
docker-compose up --build
```

This will start all three services:
- WebGateway: http://localhost:5000
- GalleryService: http://localhost:5001
- CheckoutService: http://localhost:5002

### Option 2: Running Individual Services

#### GalleryService
```bash
cd Services/GalleryService
dotnet run
```

#### CheckoutService
```bash
cd Services/CheckoutService
dotnet run
```

#### WebGateway
```bash
cd Services/WebGateway
dotnet run
```

**Note:** Make sure to update `appsettings.json` in WebGateway with correct service URLs if running individually.

## API Endpoints

### GalleryService

- `GET /api/gallery` - Get all gallery items
- `GET /api/gallery/{id}` - Get gallery item by ID
- `POST /api/gallery/items` - Get multiple items by IDs
- `GET /health` - Health check

### CheckoutService

- `POST /api/checkout/pay` - Process payment
- `GET /api/checkout/order/{orderId}` - Get order status
- `GET /health` - Health check

## Service Communication

- **WebGateway** communicates with **GalleryService** and **CheckoutService** via HTTP REST APIs
- Services use shared models from `Shared/SharedModels` project
- CORS is configured to allow frontend access

## Docker

Each service has its own Dockerfile:
- `Services/GalleryService/Dockerfile`
- `Services/CheckoutService/Dockerfile`
- `Services/WebGateway/Dockerfile`

## Deployment

The `azure-pipelines.yml` can be updated to build and deploy each microservice independently. Each service can be:
- Scaled independently
- Deployed to different environments
- Updated without affecting other services

## Benefits of Microservices Architecture

1. **Independent Deployment** - Each service can be deployed separately
2. **Scalability** - Scale services independently based on load
3. **Technology Diversity** - Each service can use different technologies if needed
4. **Fault Isolation** - Failure in one service doesn't bring down the entire system
5. **Team Autonomy** - Different teams can work on different services
