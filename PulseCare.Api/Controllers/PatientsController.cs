using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class PatientsController : ControllerBase
{
    private readonly IPatientRepository _patientRepository;

    public PatientsController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    // GET: /patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
    {
        var patients = await _patientRepository.GetAllPatientsAsync();

        var patientsDto = patients.Select(p => new PatientDto
        {
            Name = p.User?.Name,
            Email = p.User?.Email,
            Phone = p.EmergencyContact?.Phone,
            Conditions = p.Conditions.Select(c => c.Name).ToList()
        }).ToList();

        return Ok(patientsDto);
    }

    // GET: /patients/{patientId}/overview
    [HttpGet("{patientId}/overview")]
    public async Task<ActionResult<PatientOverviewDto>> GetPatientOverview(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient == null)
            return NotFound();

        var overviewDto = new PatientOverviewDto
        {
            Name = patient.User?.Name,
            Email = patient.User?.Email,
            Phone = patient.EmergencyContact?.Phone,
            DateOfBirth = patient.DateOfBirth,
            CreatedAt = patient.CreatedAt,
            BloodType = patient.BloodType,
            Conditions = patient.Conditions.Select(c => c.Name).ToList(),
            Allergies = patient.Allergies.Select(a => a.Name).ToList()
        };

        return Ok(overviewDto);
    }
}
