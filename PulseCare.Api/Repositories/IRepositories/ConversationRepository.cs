using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Communication;

public class ConversationRepository : IConversationRepository
{
    private readonly PulseCareDbContext _context;

    public ConversationRepository(PulseCareDbContext context)
    {
        _context = context;
    }

    public async Task<Conversation?> GetByIdAsync(Guid id)
    {
        return await _context.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Conversation> GetOrCreateForPatientAndDoctorAsync(Guid patientId, Guid doctorId)
    {
        var convo = await _context.Conversations
            .FirstOrDefaultAsync(c => c.PatientId == patientId && c.DoctorId == doctorId);

        if (convo is not null)
            return convo;

        convo = new Conversation
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            DoctorId = doctorId
        };

        _context.Conversations.Add(convo);
        await _context.SaveChangesAsync();

        return convo;
    }

    public async Task<List<Conversation>> GetConversationsForPatientAsync(Guid patientId)
    {
        return await _context.Conversations
            .Where(c => c.PatientId == patientId)
            .Include(c => c.Messages)
            .ToListAsync();
    }


    public async Task<List<Conversation>> GetConversationsForDoctorAsync(Guid userId)
    {
        return await _context.Conversations
            .Where(c => c.DoctorId == userId)
            .Include(c => c.Messages)
            .ToListAsync();
    }
}

