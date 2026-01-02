using Data.Dtos;

public class NoteRepository : INoteRepository
{
    public Task<IEnumerable<NoteDto>> GetAllByIdAsync(Guid patientId)
    {
        throw new NotImplementedException();
    }
}