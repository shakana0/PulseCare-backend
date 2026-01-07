using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Communication;

public class NoteRepository : INoteRepository
{
    private readonly PulseCareDbContext _context;

    public NoteRepository(PulseCareDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddNoteAsync(Note note)
    {
        try
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<IEnumerable<Note>> GetAllByClerkUserIdAsync(string clerkUserId)
    {
        return await _context.Notes
            .AsNoTracking()
            .Include(n => n.Doctor)
                .ThenInclude(d => d.User)
            .Include(n => n.Appointment)
            .Include(n => n.Patient)
                .ThenInclude(p => p.User)
            .Where(n => n.Patient.User.ClerkId == clerkUserId)
            .ToListAsync();
    }
}