public record ConversationDto(
    Guid Id,
    Guid PatientId,
    Guid DoctorId,
    MessageDto? LatestMessage,
    int UnreadCount
);
