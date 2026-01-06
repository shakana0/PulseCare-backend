using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Medical;

public interface IHealthStatRepository
{
    Task<ActionResult<List<HealthStatsDto>>> GetHealthStatsAsync(Guid id);
    Task<HealthStat> CreateHealthStatsAsync(HealthStat healthStat);
    Task<HealthStat?> GetHealthStatByIdAsync(Guid healthStatId);
    Task<HealthStat?> UpdateHealthStatAsync(Guid healthStatId, UpdateHealthStatDto updateDto);
    Task<bool> DeleteHealthStatAsync(Guid healthStatId);
}