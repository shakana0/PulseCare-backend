using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace PulseCare.API.Data.Entities.Medical;
   public class HealthTip
    {
        public Guid Id { get; set; }
        public HealthTipCategoryType Category { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Icon { get; set; }
        public Guid PatientId { get; set; }
        public Patient? Patient { get; set; }
    }