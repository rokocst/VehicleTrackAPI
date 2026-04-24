using System.ComponentModel.DataAnnotations;

namespace VehicleTrackAPI.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required]
    [StringLength(17, MinimumLength = 11, ErrorMessage = "VIN must be between 11 and 17 characters.")]
    public string VIN { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Make { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int Year { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative.")]
    public int Mileage { get; set; }

    [Required]
    [RegularExpression("^(Active|Maintenance|Retired)$",
        ErrorMessage = "Status must be Active, Maintenance, or Retired.")]
    public string Status { get; set; } = "Active";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
