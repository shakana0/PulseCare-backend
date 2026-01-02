using PulseCare.API.Data.Entities.Medical;
public interface IMedicationRepository
{
    Task<IEnumerable<Medication>> GetMedicationsByIdAsync(Guid id);
}