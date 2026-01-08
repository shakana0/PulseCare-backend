public class AppointmentDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Time { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? DoctorName { get; set; }
    public string? PatientName { get; set; }
    public string? Reason { get; set; }
    public List<string> Notes { get; set; } = new();
}
