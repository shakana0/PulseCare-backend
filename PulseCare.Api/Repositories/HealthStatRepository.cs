using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Context;

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
}