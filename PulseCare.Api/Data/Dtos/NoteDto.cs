public class NoteDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? DoctorName { get; set; }
    public DateTime Date { get; set; }
    public string? Content { get; set; }
}