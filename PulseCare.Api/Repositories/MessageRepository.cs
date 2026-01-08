using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Communication;
using Microsoft.EntityFrameworkCore;
public class MessageRepository : IMessageRepository
{
    private readonly PulseCareDbContext _context;

    public MessageRepository(PulseCareDbContext context)
    {
        _context = context;
    }

    public async Task<Message> CreateMessageAsync(Guid conversationId, string subject, string content, bool fromPatient)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Subject = subject,
            Content = content,
            Date = DateTime.UtcNow,
            Read = false,
            FromPatient = fromPatient,
            ConversationId = conversationId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<List<Message>> GetMessagesAsync(Guid conversationId)
    {
        return await _context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.Date)
            .ToListAsync();
    }
    public async Task<bool> MarkAllAsReadAsync(Guid conversationId)
    {
        var messages = await _context.Messages
            .Where(m => m.ConversationId == conversationId && !m.Read)
            .ToListAsync();

        if (messages.Count == 0)
            return true;

        foreach (var msg in messages)
            msg.Read = true;

        await _context.SaveChangesAsync();
        return true;
    }

}