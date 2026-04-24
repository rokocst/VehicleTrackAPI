using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VehicleTrackAPI.Models;

namespace VehicleTrackAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var d = new DateTime(2025, 1, 1);
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle { Id = 1, VIN = "1HGCM82633A123456", Make = "Honda", Model = "Civic", Year = 2018, Mileage = 45000, Status = "Active", CreatedAt = d },
            new Vehicle { Id = 2, VIN = "2T1BURHE0JC034567", Make = "Toyota", Model = "Corolla", Year = 2019, Mileage = 62000, Status = "Maintenance", CreatedAt = d },
            new Vehicle { Id = 3, VIN = "3VWFE21C04M000001", Make = "Ford", Model = "F-150", Year = 2021, Mileage = 29000, Status = "Active", CreatedAt = d },
            new Vehicle { Id = 4, VIN = "4T1BF1FK5EU456789", Make = "Toyota", Model = "Camry", Year = 2015, Mileage = 110000, Status = "Retired", CreatedAt = d },
            new Vehicle { Id = 5, VIN = "5NPE34AF1FH012345", Make = "Honda", Model = "Accord", Year = 2020, Mileage = 38000, Status = "Active", CreatedAt = d }
        );
    }
}