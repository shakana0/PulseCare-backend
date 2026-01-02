using PulseCare.API.Data.Entities.Medical;

public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetMedicationsByIdAsync(Guid id);
    Task<Medication> CreateMedicationAsync(Medication medication);
    Task<Medication?> GetMedicationByIdAsync(Guid medicationId);
    Task<Medication?> UpdateMedicationAsync(Guid medicationId, Medication medication);
}