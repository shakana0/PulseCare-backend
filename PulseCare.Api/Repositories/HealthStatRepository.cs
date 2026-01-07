using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulseCare.API.Context;
using PulseCare.API.Data.Entities.Medical;

public class HealthStatRepository : IHealthStatRepository
{
    private readonly PulseCareDbContext _context;
    public HealthStatRepository(PulseCareDbContext context)
    {
        _context = context;
    }
    public async Task<ActionResult<List<HealthStatsDto>>> GetHealthStatsAsync(Guid id)
    {
        var healthData = _context.HealthStats.Where(h => h.PatientId == id).ToList();

        if (!healthData.Any()) return new NotFoundResult();

        var response = new List<HealthStatsDto>();

        foreach (var item in healthData)
        {
            response.Add(
                new HealthStatsDto
                    (
                        item.Id,
                        item.Type,
                        item.Value,
                        item.Unit,
                        item.Date,
                        item.Status
                    )
            );
        }

        return response;
    }

    public async Task<List<HealthStatsDto>> GetMyHealthStatsAsync(string clerkId)
    {
        var healthData = await _context.HealthStats.Include(hs => hs.Patient).ThenInclude(p => p.User).Where(h => h.Patient.User.ClerkId == clerkId).ToListAsync();

        if (!healthData.Any()) return new List<HealthStatsDto>();

        var response = new List<HealthStatsDto>();

        foreach (var item in healthData)
        {
            response.Add(
                new HealthStatsDto
                    (
                        item.Id,
                        item.Type,
                        item.Value,
                        item.Unit,
                        item.Date,
                        item.Status
                    )
            );
        }

        return response;
    }

    public async Task<HealthStat?> GetHealthStatByIdAsync(Guid healthStatId)
    {
        return await _context.HealthStats.FindAsync(healthStatId);
    }

    public async Task<HealthStat> CreateHealthStatsAsync(HealthStat healthStat)
    {
        await _context.HealthStats.AddAsync(healthStat);
        await _context.SaveChangesAsync();
        return healthStat;

    }

    public async Task<HealthStat?> UpdateHealthStatAsync(Guid healthStatId, UpdateHealthStatDto updateDto)
    {
        var existingHealthStat = await _context.HealthStats.FindAsync(healthStatId);

        if (existingHealthStat == null)
            return null;

        if (updateDto.Type.HasValue)
            existingHealthStat.Type = updateDto.Type.Value;

        if (updateDto.Value != null)
            existingHealthStat.Value = updateDto.Value;

        if (updateDto.Unit != null)
            existingHealthStat.Unit = updateDto.Unit;

        if (updateDto.Date.HasValue)
            existingHealthStat.Date = updateDto.Date.Value;

        if (updateDto.Status.HasValue)
            existingHealthStat.Status = updateDto.Status.Value;

        await _context.SaveChangesAsync();
        return existingHealthStat;
    }

    public async Task<bool> DeleteHealthStatAsync(Guid healthStatId)
    {
        var healthStat = await _context.HealthStats.FindAsync(healthStatId);

        if (healthStat == null)
            return false;

        _context.HealthStats.Remove(healthStat);
        await _context.SaveChangesAsync();
        return true;
    }
}