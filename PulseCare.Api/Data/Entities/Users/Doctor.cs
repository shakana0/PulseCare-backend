using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;

namespace PulseCare.API.Data.Entities.Users;

public class Doctor
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public required string Specialty { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}
