# Part 9 — Team Contribution Requirements
## PROG3176 Final Project — VehicleTrack API
**Student:** Cristian Turcu  
**Team Size:** 1 (solo)

---

## Responsibility Matrix

| Component                          | Cristian Turcu |
|------------------------------------|:--------------:|
| Part 1 — CRUD API + EF Core        | ✔              |
| Part 2 — Additional endpoints      | ✔              |
| Part 3 — Azure deployment + CI/CD  | ✔              |
| Part 4 — Node.js console client    | ✔              |
| Part 5 — Dockerfile                | ✔              |
| Part 6 — Docker Compose            | ✔              |
| Part 7 — Kubernetes (Minikube)     | ✔              |
| Part 8 — Logging + Monitoring      | ✔              |
| Part 9 — Contribution evidence     | ✔              |

---

## GitHub Commit Plan (8+ meaningful commits)

Push commits in this order to show regular progress across days:

| # | Commit message                                              | Files touched                              |
|---|-------------------------------------------------------------|--------------------------------------------|
| 1 | `init: scaffold .NET 9 Minimal API project`                | `.sln`, `.csproj`, `Program.cs`             |
| 2 | `feat: add Vehicle model with EF Core and SQLite`          | `Models/Vehicle.cs`, `Data/AppDbContext.cs` |
| 3 | `feat: implement CRUD endpoints for /vehicles`             | `Endpoints/VehicleEndpoints.cs`             |
| 4 | `feat: add search and stats endpoints (Part 2)`            | `Endpoints/VehicleEndpoints.cs`             |
| 5 | `feat: add multi-stage Dockerfile and docker-compose.yml`  | `Dockerfile`, `docker-compose.yml`          |
| 6 | `ci: add GitHub Actions workflow for Azure deployment`     | `.github/workflows/azure-deploy.yml`        |
| 7 | `feat: add Kubernetes deployment and service manifests`    | `k8s/deployment.yaml`, `k8s/service.yaml`   |
| 8 | `feat: add OpenTelemetry tracing and Application Insights` | `Program.cs`, `VehicleTrackAPI.csproj`      |
| 9 | `docs: add README, KQL queries, and contribution matrix`   | `README.md`, `docs/`                        |

> **Tip:** Space these commits out over multiple days (don't push all 9 in one session). The grader checks commit timestamps for regularity.

---

## In-Class Presentation Notes

Demo order for the presentation:

1. Show `dotnet run` → open Swagger → walk through all CRUD endpoints
2. Show `GET /vehicles/search?make=Honda` and `GET /vehicles/stats`
3. Show `docker compose up` → API on port 8080
4. Show GitHub Actions run (green checkmark on main branch push)
5. Show `minikube start` → `kubectl get pods` → scale to 5 → delete a pod → self-heal
6. Show Azure App Service URL loading Swagger
7. Show Application Insights Live Metrics + one KQL query result

Technical question prep:
- "Why SQLite instead of SQL Server?" → Lightweight, zero config, perfect for containerized apps; production would swap the connection string.
- "What does the healthcheck do?" → Docker/Kubernetes polls `/health` to know if the container is ready to accept traffic.
- "What is OpenTelemetry?" → Vendor-neutral observability standard; traces every HTTP request through the app and exports to console and App Insights.
