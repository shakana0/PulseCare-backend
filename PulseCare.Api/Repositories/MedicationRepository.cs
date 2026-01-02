using System.Linq;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Medical;

public class MedicationRepository : IMedicationRepository
{
    private readonly PulseCareDbContext _context;
    public MedicationRepository(PulseCareDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Medication>> GetMedicationsByIdAsync(Guid id)
    {
        return _context.Medications
                    .Where(m => m.PatientId == id)
                    .ToList();
    }

}