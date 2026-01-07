public record StartConversationRequest(
    string? PatientId,
    string? DoctorId,
    string Subject,
    string Content,
    bool FromPatient
);