using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Entities.Communication;

[ApiController]
[Route("[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientRepository _patientRepository;

    public PatientsController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    // GET: /patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientsDto>>> GetPatients()
    {
        var patients = await _patientRepository.GetAllPatientsAsync();

        foreach (var patient in patients)
        {
            System.Console.WriteLine($"{patient.Id}");
        }

        var patientsDto = patients.Select(p => new PatientsDto
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

    // GET: /patients/{patientId}/appointments
    [HttpGet("{patientId}/appointments")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetPatientAppointments(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient == null)
            return NotFound();

        var appointmentsDto = patient.Appointments.Select(a => new AppointmentDto
        {
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            Reason = a.Comment,
            Notes = patient.Notes
                       .Where(n => n.AppointmentId == a.Id)
                       .Select(n => n.Content)
                       .ToList()
        }).ToList();

        return Ok(appointmentsDto);
    }

    // GET: /patients/{patientId}/medications
    [HttpGet("{patientId}/medications")]
    public async Task<ActionResult<IEnumerable<MedicationDto>>> GetPatientMedications(Guid patientId)
    {
        var patient = await _patientRepository.GetPatientByIdAsync(patientId);

        if (patient == null)
            return NotFound();

        var medicationsDto = patient.Medications.Select(m => new MedicationDto
        {
            Name = m.Name,
            Dosage = m.Dosage,
            Frequency = m.Frequency,
            Instructions = m.Instructions
        }).ToList();

        return Ok(medicationsDto);
    }
}
