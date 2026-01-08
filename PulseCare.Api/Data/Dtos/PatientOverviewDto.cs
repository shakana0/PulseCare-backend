using PulseCare.API.Data.Entities.Medical;

public class PatientOverviewDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? BloodType { get; set; }
    public List<string> Conditions { get; set; } = new();
    public List<string> Allergies { get; set; } = new();
    public List<MedicationDto> Medications { get; set; } = new();
    public List<AppointmentDto> Appointments { get; set; } = new();
    public List<HealthStatsDto> HealthStats { get; set; } = new();
    public EmergencyContactDto? EmergencyContact { get; set; }
}

public class EmergencyContactDto
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Relationship { get; set; }
}
