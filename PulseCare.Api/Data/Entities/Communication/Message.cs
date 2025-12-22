using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Data.Entities.Communication;
public class Message
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        public Guid? DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public required string Subject { get; set; }
        public required string Content { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public bool FromPatient { get; set; }
    }