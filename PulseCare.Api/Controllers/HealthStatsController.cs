using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Medical;

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
    public async Task<ActionResult<List<HealthStatsDto>>> GetPatientHealthStats(Guid patientId)
    {
        var healthStats = await _repository.GetHealthStatsAsync(patientId);

        return Ok(healthStats);
    }

    [HttpGet]
    public async Task<ActionResult<List<HealthStatsDto>>> GetMyHealthStats()
    {
        var clerkUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(clerkUserId))
        {
            return Unauthorized("User not authenticated");
        }

        var healthStats = await _repository.GetMyHealthStatsAsync(clerkUserId);

        return Ok(healthStats);
    }


    [HttpPost("{patientId}")]
    public async Task<ActionResult<HealthStatsDto>> CreateHealthStats(Guid patientId, [FromBody] CreateHealtStatDto createHealthStatDto)
    {
        var healthStat = new HealthStat
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Type = createHealthStatDto.Type,
            Value = createHealthStatDto.Value,
            Unit = createHealthStatDto.Unit,
            Date = DateTime.UtcNow,
            Status = createHealthStatDto.Status
        };

        var createdHealthStat = await _repository.CreateHealthStatsAsync(healthStat);

        return CreatedAtAction(nameof(GetPatientHealthStats), new { patientId }, new HealthStatsDto(
            createdHealthStat.Id,
            createdHealthStat.Type,
            createdHealthStat.Value,
            createdHealthStat.Unit,
            createdHealthStat.Date,
            createdHealthStat.Status
        ));
    }

    [HttpPatch("{healthStatId}")]
    public async Task<ActionResult<HealthStatsDto>> UpdateHealthStat(Guid healthStatId, [FromBody] UpdateHealthStatDto updateHealthStatDto)
    {
        var updatedHealthStat = await _repository.UpdateHealthStatAsync(healthStatId, updateHealthStatDto);

        if (updatedHealthStat == null)
            return NotFound();

        return Ok(new HealthStatsDto(
            updatedHealthStat.Id,
            updatedHealthStat.Type,
            updatedHealthStat.Value,
            updatedHealthStat.Unit,
            updatedHealthStat.Date,
            updatedHealthStat.Status
        ));
    }

    [HttpDelete("{healthStatId}")]
    public async Task<ActionResult> DeleteHealthStat(Guid healthStatId)
    {
        var deleted = await _repository.DeleteHealthStatAsync(healthStatId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

