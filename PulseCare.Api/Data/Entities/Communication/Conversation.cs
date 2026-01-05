using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Data.Entities.Communication;

public class Conversation
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }

    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }

    public List<Message> Messages { get; set; } = new();
}

