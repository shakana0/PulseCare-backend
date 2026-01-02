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

    public async Task<IEnumerable<Note>> GetAllByIdAsync(Guid? patientId)
    {
        var response = await _context.Notes
            .Include(n => n.Doctor)
                .ThenInclude(d => d.User)
            .Include(n => n.Appointment)
            .Where(n => n.PatientId == patientId)
            .ToListAsync();

        return response;
    }
}