using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Medical;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly PulseCareDbContext _context;

    public AppointmentRepository(PulseCareDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.AppointmentNotes)
            .Where(a => a.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.AppointmentNotes)
            .ToListAsync();
    }

    public async Task<Appointment?> GetAppointmentByAppointmentIdAsync(Guid appointmentId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.AppointmentNotes)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointment)
    {
        var existingAppointment = await _context.Appointments.FindAsync(appointment.Id);

        if (existingAppointment == null)
        {
            return null;
        }

        _context.Entry(existingAppointment).CurrentValues.SetValues(appointment);
        await _context.SaveChangesAsync();
        return existingAppointment;
    }

    public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Appointment>> GetDoctorAppointmentsByClerkId(string clerkId)
    {
        return await _context.Appointments.Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.AppointmentNotes)
            .Where(a => a.Doctor.User.ClerkId == clerkId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetPatientAppointmentsByClerkId(string clerkId)
    {
        return await _context.Appointments.Include(a => a.Doctor)
            .ThenInclude(d => d.User)
            .Include(a => a.Patient)
            .ThenInclude(p => p.User)
            .Include(a => a.AppointmentNotes)
            .Where(a => a.Patient.User.ClerkId == clerkId)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetDoctorsAppointmentsAsync(string clerkId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
                .ThenInclude(p => p.User)
            .Include(a => a.Doctor)
                .ThenInclude(d => d.User)
            .Include(a => a.AppointmentNotes)
            .Where(a => a.Doctor.User.ClerkId == clerkId)
            .ToListAsync();
    }
}