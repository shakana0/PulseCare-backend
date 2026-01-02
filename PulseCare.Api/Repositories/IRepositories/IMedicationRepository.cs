using PulseCare.API.Data.Entities.Medical;

public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetMedicationsByPatientIdAsync(Guid patientId);
    Task<Medication> CreateMedicationAsync(Medication medication);
    Task<Medication?> GetMedicationByIdAsync(Guid medicationId);
    Task<Medication?> UpdateMedicationAsync(Guid medicationId, Medication medication);
    Task<bool> DeleteMedicationAsync(Guid medicationId);
}