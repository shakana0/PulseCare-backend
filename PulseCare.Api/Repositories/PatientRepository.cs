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
            .Include(p => p.Conditions)
            .Include(p => p.EmergencyContact)
            .ToArrayAsync();
    }

    public async Task<Patient?> GetPatientByIdAsync(Guid patientId)
    {
        return await _context.Patients
             .Include(p => p.User)
             .Include(p => p.EmergencyContact)
             .Include(p => p.Conditions)
             .Include(p => p.Allergies)
             .Include(p => p.Medications)
             .Include(p => p.HealthStats)
             .Include(p => p.Appointments)
                .ThenInclude(a => a.AppointmentNotes)
             .Include(p => p.Appointments)
                 .ThenInclude(a => a.Doctor)
                     .ThenInclude(d => d.User)
             .Include(p => p.Notes)
                 .ThenInclude(n => n.Doctor)
                     .ThenInclude(d => d.User)
             .FirstOrDefaultAsync(p => p.Id == patientId);
    }

    public async Task<Patient?> GetPatientByClerkIdAsync(string clerkId)
    {
        return await _context.Patients
            .Include(p => p.User)
            .Include(p => p.EmergencyContact)
            .Include(p => p.Conditions)
            .Include(p => p.Allergies)
            .Include(p => p.Medications)
            .Include(p => p.HealthStats)
            .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
                    .ThenInclude(d => d.User)
            .Include(p => p.Notes)
                .ThenInclude(n => n.Doctor)
                    .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(p => p.User.ClerkId == clerkId);
    }
}
