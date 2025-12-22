using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace PulseCare.API.Data.Entities.Medical;
   public class HealthStat
    {
        public Guid Id { get; set; }
        public HealthStatType Type { get; set; }
        public required string Value { get; set; }
        public required string Unit { get; set; }
        public DateTime Date { get; set; }
        public HealthStatusType Status { get; set; }

        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
