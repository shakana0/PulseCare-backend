using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentsController(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
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
}

