using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using VehicleTrackAPI.Data;
using VehicleTrackAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// ── Part 8.1: Structured Logging ─────────────────────────────────────────────
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// ── Services ──────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "VehicleTrack API",
        Version     = "v1",
        Description = "Fleet vehicle and maintenance tracking API — PROG3176 Final Project"
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=vehicletrack.db"));

// ── Part 8.2: OpenTelemetry Tracing ──────────────────────────────────────────
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(serviceName: "VehicleTrackAPI", serviceVersion: "1.0.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddConsoleExporter());

// ── Part 8.3: Azure Application Insights ─────────────────────────────────────
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// ── App ───────────────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VehicleTrack API v1");
    c.RoutePrefix = "swagger";
});

// Health check endpoint (Docker Compose + Kubernetes probes)
app.MapGet("/health", (ILogger<Program> logger) =>
{
    logger.LogInformation("Health check called at {Time}", DateTime.UtcNow);
    return Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
})
.WithTags("Health")
.WithSummary("Health check");

// Redirect root to Swagger
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Register all vehicle endpoints
app.MapVehicleEndpoints();

// Apply EF Core migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    db.Database.Migrate();
    logger.LogInformation("Database migrated successfully. VehicleTrack API starting up.");
}

app.Run();
