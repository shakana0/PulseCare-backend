namespace PulseCare.API.Data.Dtos;

public class CreateAppointmentDto
{
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; } 
    public DateTime Date { get; set; }
    public string Time { get; set; } = string.Empty; 
    public string Type { get; set; } = "Checkup";
    public string? Reason { get; set; }
}
