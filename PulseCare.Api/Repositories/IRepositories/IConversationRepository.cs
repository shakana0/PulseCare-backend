using PulseCare.API.Data.Entities.Communication;

public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(Guid id);
    Task<Conversation> GetOrCreateForPatientAndDoctorAsync(Guid patientId, Guid doctorId);
    Task<List<Conversation>> GetConversationsForPatientAsync(Guid patientId);
    Task<List<Conversation>> GetConversationsForDoctorAsync(Guid doctorId);
}