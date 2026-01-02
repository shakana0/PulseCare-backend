using Microsoft.AspNetCore.Routing.Patterns;

namespace Data.Dtos;

public class NoteDto
{
    public Guid NoteId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string AppointmentDetails { get; set; } = string.Empty;
}