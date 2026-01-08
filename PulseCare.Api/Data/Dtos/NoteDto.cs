public class NoteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string AppointmentDetails { get; set; } = string.Empty;
}