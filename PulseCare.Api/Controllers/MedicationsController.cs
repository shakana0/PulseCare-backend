using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Medical;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicationsController : ControllerBase
{
    private readonly IMedicationRepository _medicationRepository;

    public MedicationsController(IMedicationRepository medicationRepository)
    {
        _medicationRepository = medicationRepository;
    }

    // GET: /api/medications/me
    [HttpGet("me")]
    public async Task<ActionResult<IEnumerable<MedicationDto>>> GetMyMedications()
    {
        var clerkId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(clerkId))
            return Unauthorized("Unable to identify user from token");

        var medications = await _medicationRepository.GetMedicationsByClerkIdAsync(clerkId);

        var medicationsDto = medications.Select(m => new MedicationDto(
            m.Id,
            m.Name,
            m.Dosage,
            m.Frequency,
            m.Instructions,
            m.TimesPerDay,
            m.StartDate,
            m.EndDate
        )).ToList();

        return Ok(medicationsDto);
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicationDto>>> GetPatientMedications(Guid patientId)
    {
        var medications = await _medicationRepository.GetMedicationsByPatientIdAsync(patientId);

        if (medications == null)
            return NotFound();

        var medicationsDto = medications.Select(m => new MedicationDto(
            m.Id,
            m.Name,
            m.Dosage,
            m.Frequency,
            m.Instructions,
            m.TimesPerDay,
            m.StartDate,
            m.EndDate
        )).ToList();

        return Ok(medicationsDto);
    }

    [HttpPost("{patientId}")]
    public async Task<ActionResult<MedicationDto>> CreateMedication(Guid patientId, [FromBody] CreateMedicationDto createMedicationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var medication = new Medication
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Name = createMedicationDto.Name,
            Dosage = createMedicationDto.Dosage,
            Frequency = createMedicationDto.Frequency,
            Instructions = createMedicationDto.Instructions,
            TimesPerDay = createMedicationDto.TimesPerDay,
            StartDate = createMedicationDto.StartDate,
            EndDate = createMedicationDto.EndDate
        };

        var createdMedication = await _medicationRepository.CreateMedicationAsync(medication);

        return CreatedAtAction(nameof(GetPatientMedications), new { patientId }, new MedicationDto(
            createdMedication.Id,
            createdMedication.Name,
            createdMedication.Dosage,
            createdMedication.Frequency,
            createdMedication.Instructions,
            createdMedication.TimesPerDay,
            createdMedication.StartDate,
            createdMedication.EndDate
        ));
    }

    [HttpPut("{medicationId}")]
    public async Task<ActionResult<MedicationDto>> UpdateMedication(Guid medicationId, [FromBody] UpdateMedicationDto updateMedicationDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medication = new Medication
        {
            Name = updateMedicationDto.Name,
            Dosage = updateMedicationDto.Dosage,
            Frequency = updateMedicationDto.Frequency,
            Instructions = updateMedicationDto.Instructions,
            TimesPerDay = updateMedicationDto.TimesPerDay,
            StartDate = updateMedicationDto.StartDate,
            EndDate = updateMedicationDto.EndDate
        };

        var updatedMedication = await _medicationRepository.UpdateMedicationAsync(medicationId, medication);

        if (updatedMedication == null)
            return NotFound();

        return Ok(new MedicationDto(
            updatedMedication.Id,
            updatedMedication.Name,
            updatedMedication.Dosage,
            updatedMedication.Frequency,
            updatedMedication.Instructions,
            updatedMedication.TimesPerDay,
            updatedMedication.StartDate,
            updatedMedication.EndDate
        ));
    }

    [HttpDelete("{medicationId}")]
    public async Task<ActionResult> DeleteMedication(Guid medicationId)
    {
        var deleted = await _medicationRepository.DeleteMedicationAsync(medicationId);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}


