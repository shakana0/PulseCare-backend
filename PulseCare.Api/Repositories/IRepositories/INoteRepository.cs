using PulseCare.API.Data.Entities.Communication;

public interface INoteRepository
{
    Task<bool> AddNoteAsync(Note note);
    Task<IEnumerable<Note>> GetAllByClerkUserIdAsync(string clerkUserId);
}