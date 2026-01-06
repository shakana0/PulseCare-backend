using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
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
            Id = p.Id,
            Name = p.User?.Name ?? "Unknown",
            Email = p.User?.Email ?? "No email",
            Phone = p.EmergencyContact?.Phone,
            Conditions = p.Conditions?.Select(c => c.Name).ToList() ?? new List<string>()
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
            Name = patient.User?.Name ?? "Unknown",
            Email = patient.User?.Email ?? "No email",
            Phone = patient.EmergencyContact?.Phone,
            DateOfBirth = patient.DateOfBirth,
            CreatedAt = patient.CreatedAt,
            BloodType = patient.BloodType ?? "Unknown",
            Conditions = patient.Conditions?.Select(c => c.Name).ToList() ?? new List<string>(),
            Allergies = patient.Allergies?.Select(a => a.Name).ToList() ?? new List<string>(),
            Medications = patient.Medications?.Select(m => new MedicationDto(
                m.Id,
                m.Name,
                m.Dosage,
                m.Frequency,
                m.Instructions,
                m.TimesPerDay,
                m.StartDate
            )).ToList() ?? new List<MedicationDto>(),
            Appointments = patient.Appointments?.Select(a => new AppointmentDto
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time.ToString(@"hh\:mm"),
                Type = a.Type.ToString(),
                Status = a.Status.ToString(),
                DoctorName = a.Doctor?.User?.Name,
                PatientName = patient.User?.Name,
                Reason = a.Comment,
                Notes = a.AppointmentNotes?.Select(n => n.Content).ToList() ?? new List<string>()
            }).ToList() ?? new List<AppointmentDto>(),
            HealthStats = patient.HealthStats?.Select(h => new HealthStatsDto(
                h.Id,
                h.Type,
                h.Value,
                h.Unit,
                h.Date,
                h.Status
            )).ToList() ?? new List<HealthStatsDto>(),
            EmergencyContact = patient.EmergencyContact != null ? new EmergencyContactDto
            {
                Name = patient.EmergencyContact.Name,
                Phone = patient.EmergencyContact.Phone,
                Relationship = patient.EmergencyContact.Relationship
            } : null
        };

        return Ok(overviewDto);
    }
}
