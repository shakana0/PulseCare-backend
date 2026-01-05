using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Medical;

public class MedicationRepository : IMedicationRepository
{
    private readonly PulseCareDbContext _context;
    public MedicationRepository(PulseCareDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Medication>> GetMedicationsByPatientIdAsync(Guid id)
    {
        return _context.Medications
                    .Where(m => m.PatientId == id)
                    .ToList();
    }

    public async Task<Medication> CreateMedicationAsync(Medication medication)
    {
        await _context.Medications.AddAsync(medication);
        await _context.SaveChangesAsync();
        return medication;
    }

    public async Task<Medication?> GetMedicationByIdAsync(Guid medicationId)
    {
        return await _context.Medications.FindAsync(medicationId);
    }

    public async Task<Medication?> UpdateMedicationAsync(Guid medicationId, Medication medication)
    {
        var existingMedication = await _context.Medications.FindAsync(medicationId);

        if (existingMedication == null)
            return null;

        existingMedication.Name = medication.Name;
        existingMedication.Dosage = medication.Dosage;
        existingMedication.Frequency = medication.Frequency;
        existingMedication.Instructions = medication.Instructions;
        existingMedication.TimesPerDay = medication.TimesPerDay;
        existingMedication.StartDate = medication.StartDate;

        await _context.SaveChangesAsync();
        return existingMedication;
    }

    public async Task<bool> DeleteMedicationAsync(Guid medicationId)
    {
        var medication = await _context.Medications.FindAsync(medicationId);

        if (medication == null)
            return false;

        _context.Medications.Remove(medication);
        await _context.SaveChangesAsync();
        return true;
    }
}