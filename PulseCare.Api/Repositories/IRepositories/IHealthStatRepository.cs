using Microsoft.AspNetCore.Mvc;

public interface IHealthStatRepository
{
    Task<ActionResult<List<HealthStatsDto>>> GetHealthStatsAsync(Guid id);
}