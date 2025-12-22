using PulseCare.API.Data.Entities.Users;

namespace PulseCare.API.Data.Entities.Medical;
 public class Condition
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
    }