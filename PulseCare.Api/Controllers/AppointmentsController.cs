using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Dtos;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Enums;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUserRepository _userRepository;

    public AppointmentsController(IAppointmentRepository appointmentRepository, IUserRepository userRepository)
    {
        _appointmentRepository = appointmentRepository;
        _userRepository = userRepository;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments()
    {
        var appointments = await _appointmentRepository.GetAllAppointmentsAsync();

        var appointmentsDto = appointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            PatientName = a.Patient?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Select(n => n.Content)
                       .ToList()
        }).ToList();

        return Ok(appointmentsDto);
    }

    [Authorize]
    [HttpGet("{patientId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetPatientAppointments(Guid patientId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAsync(patientId);

        var appointmentsDto = appointments.Select(a => new AppointmentDto
        {
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Select(n => n.Content)
                       .ToList()
        }).ToList();

        return Ok(appointmentsDto);
    }
    [Authorize]
    [HttpGet("all/patient")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllPatientAppointments()
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(clerkId))
        {
            return Unauthorized();
        }

        var patientAppointments = await _appointmentRepository.GetPatientAppointmentsByClerkId(clerkId);

        return Ok(patientAppointments.Select(a => new AppointmentDto
        {
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Select(n => n.Content)
                       .ToList()
        }).ToList());
    }

    [Authorize(Roles = "admin")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllDoctorsAppointments()
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(clerkId))
        {
            return Unauthorized();
        }

        var doctorsAppointments = await _appointmentRepository.GetDoctorAppointmentsByClerkId(clerkId);

        return Ok(doctorsAppointments.Select(a => new AppointmentDto
        {
            Id = a.Id,
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            PatientName = a.Patient?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Select(n => n.Content)
                       .ToList()
        }).ToList());
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(CreateAppointmentDto dto)
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(clerkId))
        {
            return Unauthorized();
        }

        var doctor = await _userRepository.GetDoctorWithClerkIdAsync(clerkId);
        
        if (doctor == null)
        {
            return NotFound();
        }

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            DoctorId = doctor.Id,
            Date = dto.Date,
            Time = TimeSpan.Parse(dto.Time),
            Type = Enum.Parse<AppointmentType>(dto.Type),
            Status = AppointmentStatusType.Scheduled,
            Comment = dto.Reason
        };

        var created = await _appointmentRepository.CreateAppointmentAsync(appointment);

        var resultDto = new AppointmentDto
        {
            Date = created.Date,
            Time = created.Time.ToString(@"hh\:mm"),
            Type = created.Type.ToString(),
            Status = created.Status.ToString(),
            DoctorName = created.Doctor?.User?.Name,
            Reason = created.Comment,
            Notes = new List<string>()
        };

        return CreatedAtAction(nameof(GetPatientAppointments), new { patientId = created.PatientId }, resultDto);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentRepository.GetAppointmentByAppointmentIdAsync(id);
        if (appointment == null) 
            return NotFound();

        appointment.Date = dto.Date;
        appointment.Time = dto.Time;
        appointment.Type = dto.Type;
        appointment.Status = dto.Status;
        appointment.Comment = dto.Reason;

        await _appointmentRepository.UpdateAppointmentAsync(appointment);
        return Ok(dto);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var result = await _appointmentRepository.DeleteAppointmentAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
