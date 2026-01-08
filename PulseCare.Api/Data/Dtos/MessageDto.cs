public record MessageDto(
    Guid Id,
    string Subject,
    string Content,
    DateTime Date,
    bool Read,
    bool FromPatient,
    Guid ConversationId
);
