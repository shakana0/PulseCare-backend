using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PulseCare.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HealthStatsController : ControllerBase
{
    private readonly IHealthStatRepository _repository;
    public HealthStatsController(IHealthStatRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<List<HealthStatsDto>>> GetHealthStats(Guid patientId)
    {
        var clerkUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(clerkUserId))
        {
            return Unauthorized("User not authenticated");
        }

        var healthStats = await _repository.GetHealthStatsAsync(patientId);

        return Ok(healthStats);
    }
}

