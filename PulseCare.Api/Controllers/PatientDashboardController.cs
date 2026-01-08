using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientDashboardController : ControllerBase
{
    private readonly IPatientRepository _patientRepository;

    public PatientDashboardController(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    // GET: /dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<PatientDashboardDto>> GetPatientDashboard()
    {
        var clerkUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(clerkUserId))
        {
            return Unauthorized("User not authenticated");
        }

        var patient = await _patientRepository.GetPatientByClerkIdAsync(clerkUserId);

        if (patient == null)
        {
            return NotFound();
        }

        var patientDto = new PatientDto
        {
            Id = patient.Id,
            Name = patient.User!.Name,
            Email = patient.User.Email,
            Phone = patient.EmergencyContact?.Phone,
            Conditions = patient.Conditions.Select(c => c.Name).ToList()
        };

        var healthStatsDto = patient.HealthStats
            .OrderByDescending(h => h.Date)
            .Select(h => new HealthStatsDto(
                h.Id,
                h.Type,
                h.Value,
                h.Unit,
                h.Date,
                h.Status
            ))
            .ToList();

        var medicationsDto = patient.Medications
            .Select(m => new MedicationDto(
                m.Id,
                m.Name,
                m.Dosage,
                m.Frequency,
                m.Instructions,
                m.TimesPerDay,
                m.StartDate,
                m.EndDate
            ))
            .ToList();

        var appointmentsDto = patient.Appointments
            .OrderBy(a => a.Date)
            .Select(a => new AppointmentDto
            {
                Id = a.Id,
                Type = a.Type.ToString(),
                DoctorName = a.Doctor.User.Name,
                Date = a.Date,
                Status = a.Status.ToString(),
                Time = a.Time.ToString(@"hh\:mm"),
                Reason = a.Comment,
                Notes = a.AppointmentNotes.Select(n => n.Content ?? "").ToList()
            })
            .ToList();

        var notesDto = patient.Notes
            .OrderByDescending(n => n.Date)
            .Select(n => new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                DoctorName = n.Doctor.User.Name,
                Date = n.Date,
                Content = n.Content
            })
            .ToList();

        var dashboardDto = new PatientDashboardDto
        {
            Patient = patientDto,
            HealthStats = healthStatsDto,
            Medications = medicationsDto,
            Appointments = appointmentsDto,
            Notes = notesDto
        };

        return Ok(dashboardDto);
    }
}