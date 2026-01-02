using Microsoft.AspNetCore.Mvc;

namespace PulseCare.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthStatsController : ControllerBase
{
    private readonly IHealthStatsRepository _repository;
    public HealthStatsController(IHealthStatsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<List<HealthStatsDto>>> GetHealthStats(Guid patientId)
    {
        var healthStats = await _repository.GetHealthStatsAsync(patientId);

        return Ok(healthStats);
    }
}

