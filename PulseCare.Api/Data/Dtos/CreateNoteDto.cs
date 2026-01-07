public class CreateNoteDto
{
    public string AppointmentId { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}