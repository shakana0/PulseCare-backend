using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Data.Entities.Communication;
public class Note
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public required string Title { get; set; }
        public required string Diagnosis { get; set; }
        public required string Content { get; set; }
        public DateTime Date { get; set; }
    }