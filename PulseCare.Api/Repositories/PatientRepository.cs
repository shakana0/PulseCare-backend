using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Users;

public class PatientRepository : IPatientRepository
{
    private readonly PulseCareDbContext _context;

    public PatientRepository(PulseCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
    {
        return await _context.Patients
            .Include(p => p.User)
            .Include(p => p.EmergencyContact)
            .Include(p => p.Conditions)
            .ToListAsync();
    }

    public async Task<Patient?> GetPatientByIdAsync(Guid patientId)
    {
        return await _context.Patients
            .Include(p => p.User)
            .Include(p => p.EmergencyContact)
            .Include(p => p.Conditions)
            .Include(p => p.Allergies)
            .Include(p => p.Medications)
            .Include(p => p.Appointments)
            .Include(p => p.HealthStats)
            .FirstOrDefaultAsync(p => p.Id == patientId);
    }
}
