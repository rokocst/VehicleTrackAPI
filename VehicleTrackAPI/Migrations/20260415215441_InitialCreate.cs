using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VehicleTrackAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VIN = table.Column<string>(type: "TEXT", maxLength: 17, nullable: false),
                    Make = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Mileage = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedAt", "Make", "Mileage", "Model", "Status", "VIN", "Year" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 15, 21, 54, 41, 428, DateTimeKind.Utc).AddTicks(9502), "Honda", 45000, "Civic", "Active", "1HGCM82633A123456", 2018 },
                    { 2, new DateTime(2026, 4, 15, 21, 54, 41, 428, DateTimeKind.Utc).AddTicks(9654), "Toyota", 62000, "Corolla", "Maintenance", "2T1BURHE0JC034567", 2019 },
                    { 3, new DateTime(2026, 4, 15, 21, 54, 41, 428, DateTimeKind.Utc).AddTicks(9656), "Ford", 29000, "F-150", "Active", "3VWFE21C04M000001", 2021 },
                    { 4, new DateTime(2026, 4, 15, 21, 54, 41, 428, DateTimeKind.Utc).AddTicks(9657), "Toyota", 110000, "Camry", "Retired", "4T1BF1FK5EU456789", 2015 },
                    { 5, new DateTime(2026, 4, 15, 21, 54, 41, 428, DateTimeKind.Utc).AddTicks(9659), "Honda", 38000, "Accord", "Active", "5NPE34AF1FH012345", 2020 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
