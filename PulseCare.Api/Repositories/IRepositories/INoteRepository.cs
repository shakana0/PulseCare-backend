using PulseCare.API.Data.Entities.Communication;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllByClerkUserIdAsync(string clerkUserId);
}