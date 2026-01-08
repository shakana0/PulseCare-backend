using PulseCare.API.Data.Entities.Communication;
public interface IMessageRepository
{
    Task<Message> CreateMessageAsync(Guid conversationId, string subject, string content, bool fromPatient);
    Task<List<Message>> GetMessagesAsync(Guid conversationId);
    public Task<bool> MarkAllAsReadAsync(Guid conversationId);
}