public record StartConversationRequest(
    Guid? PatientId,
    Guid? DoctorId,
    string Subject,
    string Content,
    bool FromPatient
);