using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Users;

public class DoctorRepository : IDoctorRepository
{
    private readonly PulseCareDbContext _context;

    public DoctorRepository(PulseCareDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors
            .Include(d => d.User)
            .ToListAsync();
    }
}