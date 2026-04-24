using Microsoft.EntityFrameworkCore;
using VehicleTrackAPI.Data;
using VehicleTrackAPI.Models;

namespace VehicleTrackAPI.Endpoints;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles");

        // GET /vehicles
        group.MapGet("/", async (AppDbContext db, ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching all vehicles from fleet.");
            var vehicles = await db.Vehicles.ToListAsync();
            logger.LogInformation("Returned {Count} vehicles.", vehicles.Count);
            return Results.Ok(vehicles);
        })
        .WithName("GetAllVehicles")
        .WithSummary("Get all vehicles")
        .Produces<List<Vehicle>>(200);

        // GET /vehicles/{id}
        group.MapGet("/{id:int}", async (int id, AppDbContext db, ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching vehicle with ID {Id}.", id);
            var vehicle = await db.Vehicles.FindAsync(id);
            if (vehicle is null)
            {
                logger.LogWarning("Vehicle with ID {Id} not found.", id);
                return Results.NotFound($"Vehicle with ID {id} not found.");
            }
            return Results.Ok(vehicle);
        })
        .WithName("GetVehicleById")
        .WithSummary("Get a vehicle by ID")
        .Produces<Vehicle>(200)
        .Produces(404);

        // POST /vehicles
        group.MapPost("/", async (Vehicle vehicle, AppDbContext db, ILogger<Program> logger) =>
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(vehicle);
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(vehicle, context, validationResults, true))
            {
                logger.LogWarning("Validation failed for new vehicle. Errors: {Errors}",
                    string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
                return Results.ValidationProblem(
                    validationResults
                        .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "")
                        .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "").ToArray())
                );
            }

            vehicle.CreatedAt = DateTime.UtcNow;
            db.Vehicles.Add(vehicle);
            await db.SaveChangesAsync();
            logger.LogInformation("Created new vehicle: {Make} {Model} (ID: {Id}).", vehicle.Make, vehicle.Model, vehicle.Id);
            return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
        })
        .WithName("CreateVehicle")
        .WithSummary("Create a new vehicle")
        .Produces<Vehicle>(201)
        .ProducesValidationProblem();

        // PUT /vehicles/{id}
        group.MapPut("/{id:int}", async (int id, Vehicle updated, AppDbContext db, ILogger<Program> logger) =>
        {
            var existing = await db.Vehicles.FindAsync(id);
            if (existing is null)
            {
                logger.LogWarning("Update failed — vehicle with ID {Id} not found.", id);
                return Results.NotFound($"Vehicle with ID {id} not found.");
            }

            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new System.ComponentModel.DataAnnotations.ValidationContext(updated);
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(updated, context, validationResults, true))
            {
                return Results.ValidationProblem(
                    validationResults
                        .GroupBy(v => v.MemberNames.FirstOrDefault() ?? "")
                        .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage ?? "").ToArray())
                );
            }

            existing.VIN     = updated.VIN;
            existing.Make    = updated.Make;
            existing.Model   = updated.Model;
            existing.Year    = updated.Year;
            existing.Mileage = updated.Mileage;
            existing.Status  = updated.Status;

            await db.SaveChangesAsync();
            logger.LogInformation("Updated vehicle ID {Id}: {Make} {Model}, Status={Status}.", id, existing.Make, existing.Model, existing.Status);
            return Results.Ok(existing);
        })
        .WithName("UpdateVehicle")
        .WithSummary("Update an existing vehicle")
        .Produces<Vehicle>(200)
        .Produces(404)
        .ProducesValidationProblem();

        // DELETE /vehicles/{id}
        group.MapDelete("/{id:int}", async (int id, AppDbContext db, ILogger<Program> logger) =>
        {
            var vehicle = await db.Vehicles.FindAsync(id);
            if (vehicle is null)
            {
                logger.LogWarning("Delete failed — vehicle with ID {Id} not found.", id);
                return Results.NotFound($"Vehicle with ID {id} not found.");
            }

            db.Vehicles.Remove(vehicle);
            await db.SaveChangesAsync();
            logger.LogInformation("Deleted vehicle ID {Id}: {Make} {Model}.", id, vehicle.Make, vehicle.Model);
            return Results.NoContent();
        })
        .WithName("DeleteVehicle")
        .WithSummary("Delete a vehicle")
        .Produces(204)
        .Produces(404);

        // ── Part 2: Additional functional endpoints ───────────────────────────

        // GET /vehicles/search?make=Honda&status=Active
        group.MapGet("/search", async (string? make, string? status, AppDbContext db, ILogger<Program> logger) =>
        {
            logger.LogInformation("Search called with make={Make}, status={Status}.", make, status);
            var query = db.Vehicles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(make))
                query = query.Where(v => v.Make.ToLower().Contains(make.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(v => v.Status.ToLower() == status.ToLower());

            var results = await query.ToListAsync();
            logger.LogInformation("Search returned {Count} results.", results.Count);
            return results.Count == 0
                ? Results.NotFound("No vehicles matched the search criteria.")
                : Results.Ok(results);
        })
        .WithName("SearchVehicles")
        .WithSummary("Search vehicles by make and/or status")
        .WithDescription("Filter fleet by make (partial match) and/or status (Active, Maintenance, Retired).")
        .Produces<List<Vehicle>>(200)
        .Produces(404);

        // GET /vehicles/stats
        group.MapGet("/stats", async (AppDbContext db, ILogger<Program> logger) =>
        {
            logger.LogInformation("Fleet stats endpoint called.");
            var vehicles = await db.Vehicles.ToListAsync();
            if (vehicles.Count == 0)
                return Results.Ok(new { message = "No vehicles in fleet." });

            var stats = new
            {
                TotalVehicles   = vehicles.Count,
                AverageMileage  = (int)vehicles.Average(v => v.Mileage),
                HighestMileage  = vehicles.Max(v => v.Mileage),
                LowestMileage   = vehicles.Min(v => v.Mileage),
                StatusBreakdown = vehicles.GroupBy(v => v.Status)
                                          .ToDictionary(g => g.Key, g => g.Count()),
                MakeBreakdown   = vehicles.GroupBy(v => v.Make)
                                          .OrderByDescending(g => g.Count())
                                          .ToDictionary(g => g.Key, g => g.Count()),
                OldestVehicle   = vehicles.Min(v => v.Year),
                NewestVehicle   = vehicles.Max(v => v.Year),
            };

            logger.LogInformation("Stats computed for {Count} vehicles.", vehicles.Count);
            return Results.Ok(stats);
        })
        .WithName("GetFleetStats")
        .WithSummary("Get fleet statistics")
        .WithDescription("Returns aggregate stats: total count, average mileage, status and make breakdown.")
        .Produces(200);
    }
}
