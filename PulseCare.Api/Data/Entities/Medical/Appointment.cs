using PulseCare.API.Data.Entities.Communication;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace PulseCare.API.Data.Entities.Medical;

 public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public AppointmentType Type { get; set; }
        public AppointmentStatusType Status { get; set; }
        public string? Comment  { get; set; }
        public ICollection<Note> AppointmentNotes  { get; set; } = new List<Note>();
    }
