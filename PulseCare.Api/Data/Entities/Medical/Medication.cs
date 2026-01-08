using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Data.Entities.Medical;

 public class Medication
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Dosage { get; set; }
        public required string Frequency { get; set; }
        public int TimesPerDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Instructions { get; set; }

        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
