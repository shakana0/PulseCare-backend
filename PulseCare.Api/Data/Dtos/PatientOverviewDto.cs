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
    public List<Medication> Medications { get; set; } = new();
    public List<Appointment> Appointments { get; set; } = new();
    public List<HealthStat> HealthStats { get; set; } = new();
}
