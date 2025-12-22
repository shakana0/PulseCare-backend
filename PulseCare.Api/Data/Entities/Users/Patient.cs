using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Medical;

namespace PulseCare.API.Data.Entities.Users;
public class Patient
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public User? User { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string BloodType { get; set; }

        public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
        public ICollection<Condition> Conditions { get; set; } = new List<Condition>();

        public EmergencyContact? EmergencyContact { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
        public ICollection<HealthStat> HealthStats { get; set; } = new List<HealthStat>();
        public ICollection<HealthTip> HealthTips { get; set; } = new List<HealthTip>();
    }