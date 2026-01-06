using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Users;

public class UserRepository : IUserRepository
{
    private readonly PulseCareDbContext _context;

    public UserRepository(PulseCareDbContext context)
    {
        _context = context;
    }

    public async Task AddDoctorAsync(Doctor newAdmin)
    {
        _context.Doctors.Add(newAdmin);
        await _context.SaveChangesAsync();
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<Patient?> GetPatientFromUserAsync(Guid userId)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId);

    }

    public async Task<User?> GetUserAsync(string clerkId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.ClerkId == clerkId);
    }

    public async Task RemovePatientAsync(Patient patient)
    {
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistingPatientAsync(string userId)
    {
        return await _context.Patients.Include(p => p.User).AnyAsync(u => u.User.ClerkId == userId);
    }

    public async Task<bool> IsExistingDoctorAsync(Guid userId)
    {
        return await _context.Doctors.AnyAsync(d => d.UserId == userId);
    }

    public async Task AddPatientAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
    }
}