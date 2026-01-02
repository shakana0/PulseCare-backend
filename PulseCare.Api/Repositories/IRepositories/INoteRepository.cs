using Data.Dtos;

public interface INoteRepository
{
    Task<IEnumerable<NoteDto>> GetAllByIdAsync(Guid patientId);
}