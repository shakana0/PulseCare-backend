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

    public AppointmentsController(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
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

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(CreateAppointmentDto dto)
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
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
