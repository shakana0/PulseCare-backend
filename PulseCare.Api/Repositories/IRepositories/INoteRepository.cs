using PulseCare.API.Data.Entities.Communication;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllByIdAsync(Guid? patientId);
}