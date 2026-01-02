using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Dtos;
using PulseCare.API.Data.Entities.Medical;
using PulseCare.API.Data.Enums;

[ApiController]
[Route("[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository;
    public AppointmentsController(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAllAppointments()
    {
        var appointments = await _appointmentRepository.GetAllAppointments();

        var appointmentsDto = appointments.Select(a => new AppointmentDto
        {
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Where(n => n.AppointmentId == a.Id)
                       .Select(n => n.Content)
                       .ToList()
        }).ToList();

        return Ok(appointmentsDto);
    }

    [HttpGet("{patientId}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetPatientAppointments(Guid patientId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsById(patientId);

        if (appointments == null)
            return NotFound();

        var appointmentsDto = appointments.Select(a => new AppointmentDto
        {
            Date = a.Date,
            Time = a.Time.ToString(@"hh\:mm"),
            Type = a.Type.ToString(),
            Status = a.Status.ToString(),
            DoctorName = a.Doctor?.User?.Name,
            Reason = a.Comment,
            Notes = a.AppointmentNotes
                       .Where(n => n.AppointmentId == a.Id)
                       .Select(n => n.Content)
                       .ToList()
        }).ToList();

        return Ok(appointmentsDto);
    }

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

        var created = await _appointmentRepository.CreateAppointment(appointment);

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _appointmentRepository.GetAppointmentById(id);
        if (appointment == null) 
            return NotFound();

        appointment.Date = dto.Date;
        appointment.Time = dto.Time;
        appointment.Type = dto.Type;
        appointment.Status = dto.Status;
        appointment.Comment = dto.Reason;

        await _appointmentRepository.UpdateAppointment(appointment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        var result = await _appointmentRepository.DeleteAppointment(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}

