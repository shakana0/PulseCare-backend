using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Medical;

[ApiController]
[Route("api/[controller]")]
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

    [HttpPost("{patientId}")]
    public async Task<ActionResult<MedicationDto>> CreateMedication(Guid patientId, CreateMedicationDto createMedicationDto)
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
            StartDate = createMedicationDto.StartDate
        };

        var createdMedication = await _medicationRepository.CreateMedicationAsync(medication);

        return CreatedAtAction(nameof(GetPatientMedications), new { patientId }, new MedicationDto(
            createdMedication.Id,
            createdMedication.Name,
            createdMedication.Dosage,
            createdMedication.Frequency,
            createdMedication.Instructions,
            createdMedication.TimesPerDay,
            createdMedication.StartDate
        ));
    }
}


