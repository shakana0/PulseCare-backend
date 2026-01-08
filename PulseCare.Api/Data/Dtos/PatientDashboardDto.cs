public class PatientDashboardDto
{
    public PatientDto? Patient { get; set; }
    public List<HealthStatsDto>? HealthStats { get; set; }
    public List<MedicationDto>? Medications { get; set; }
    public List<AppointmentDto>? Appointments { get; set; }
    public List<NoteDto>? Notes { get; set; }
}