# VehicleTrack API
**PROG3176 Final Project — Cristian Turcu**  
Cloud-Native Vehicle & Maintenance Tracking API  
.NET 9 Minimal API · EF Core + SQLite · Docker · Kubernetes · Azure · OpenTelemetry

---

## Quick Start

Run these in Git Bash or Command Prompt, **one at a time**:

```
cd VehicleTrackAPI\VehicleTrackAPI
dotnet restore
dotnet run
```

Open Swagger: http://localhost:5000/swagger

The SQLite database is auto-created and seeded on first run.

---

## All Endpoints

### CRUD — Part 1

| Method | Route          | Description       |
|--------|----------------|-------------------|
| GET    | /vehicles      | Get all vehicles  |
| GET    | /vehicles/{id} | Get by ID         |
| POST   | /vehicles      | Create vehicle    |
| PUT    | /vehicles/{id} | Update vehicle    |
| DELETE | /vehicles/{id} | Delete vehicle    |

### Additional — Part 2

| Method | Route                          | Description       |
|--------|--------------------------------|-------------------|
| GET    | /vehicles/search?make=&status= | Search / filter   |
| GET    | /vehicles/stats                | Fleet statistics  |

### System

| Method | Route   | Description               |
|--------|---------|---------------------------|
| GET    | /health | Health check (Docker/K8s) |

---

## Vehicle Model

```json
{
  "id": 1,
  "vin": "1HGCM82633A123456",
  "make": "Honda",
  "model": "Civic",
  "year": 2018,
  "mileage": 45000,
  "status": "Active",
  "createdAt": "2025-01-01T00:00:00Z"
}
```

**Status values:** `Active` | `Maintenance` | `Retired`

---

## Part 4 — Console Client

```
cd client
node client.js
```

Against Azure:
```
node client.js https://vehicletrack-api.azurewebsites.net
```

---

## Part 5 — Docker

```
cd VehicleTrackAPI
docker build -t vehicletrack-api .
docker run -p 8080:8080 vehicletrack-api
```

Open: http://localhost:8080/swagger

---

## Part 6 — Docker Compose

```
docker compose up --build
```

Open: http://localhost:8080/swagger
Healthcheck polls `/health` every 30 seconds.

---

## Part 7 — Kubernetes

See `docs/part7-k8s-commands.md` for the full demo script.

```
minikube start
eval $(minikube docker-env)
docker build -t vehicletrack-api:latest ./VehicleTrackAPI
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
kubectl get pods
minikube service vehicletrack-service --url
kubectl scale deployment vehicletrack-api --replicas=5
```

---

## Part 3 — Azure CI/CD

See `docs/part3-azure-setup.md` for step-by-step setup.
Push to `main` auto-triggers `.github/workflows/azure-deploy.yml`.

---

## Part 8 — Logging & Monitoring

- Structured logging in every endpoint via `ILogger<Program>`
- OpenTelemetry traces every request, exports to console
- App Insights: add connection string to `appsettings.json`
- KQL queries: see `docs/part8-kql-queries.md`

---

## Part 9 — Contribution

See `docs/part9-contribution.md` for responsibility matrix, commit plan, and presentation notes.

---

## Project Structure

```
VehicleTrackAPI/
├── .github/workflows/azure-deploy.yml   ← Part 3: CI/CD
├── VehicleTrackAPI/
│   ├── Data/AppDbContext.cs              ← EF Core + seed
│   ├── Endpoints/VehicleEndpoints.cs    ← Parts 1 & 2 + logging
│   ├── Migrations/                      ← EF Core migrations
│   ├── Models/Vehicle.cs                ← Model + validation
│   ├── Dockerfile                       ← Part 5: multi-stage
│   ├── Program.cs                       ← OTel + App Insights
│   └── appsettings.json
├── client/client.js                     ← Part 4: Node.js client
├── docs/
│   ├── part3-azure-setup.md
│   ├── part7-k8s-commands.md
│   ├── part8-kql-queries.md
│   └── part9-contribution.md
├── k8s/
│   ├── deployment.yaml                  ← Part 7: replicas: 2
│   └── service.yaml                     ← Part 7: NodePort 30080
├── docker-compose.yml                   ← Part 6: healthcheck
└── VehicleTrackAPI.sln
```
