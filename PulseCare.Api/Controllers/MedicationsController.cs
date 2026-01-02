using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{patientId}")]
    public async Task<ActionResult<IEnumerable<MedicationDto>>> GetPatientMedications(Guid patientId)
    {
        var medications = await _medicationRepository.GetMedicationsByIdAsync(patientId);

        if (medications == null)
            return NotFound();

        var medicationsDto = medications.Select(m => new MedicationDto(
            m.Id,
            m.Name,
            m.Dosage,
            m.Frequency,
            m.Instructions,
            m.TimesPerDay,
            m.StartDate
        )).ToList();

        return Ok(medicationsDto);
    }
}