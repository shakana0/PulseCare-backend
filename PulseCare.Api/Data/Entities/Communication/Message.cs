namespace PulseCare.API.Data.Entities.Communication;

public class Message
{
    public Guid Id { get; set; }
    public required string Subject { get; set; }
    public required string Content { get; set; }
    public DateTime Date { get; set; }
    public bool Read { get; set; }
    public bool FromPatient { get; set; }

    public Guid ConversationId { get; set; }
    public Conversation? Conversation { get; set; }
}