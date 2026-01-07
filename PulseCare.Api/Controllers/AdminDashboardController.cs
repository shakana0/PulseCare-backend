using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminDashboardController(
    IAppointmentRepository appointmentRepository,
    IPatientRepository patientRepository
) : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IPatientRepository _patientRepository = patientRepository;


    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<ActionResult<AdminDashboardDto>> GetAdminDashboard()
    {
        var clerkUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (clerkUserId == null)
            return Unauthorized();

        var patients = await _patientRepository.GetAllPatientsAsync();
        var appointments = await _appointmentRepository.GetDoctorsAppointmentsAsync(clerkUserId!);

        var dashboardDto = new AdminDashboardDto
        {
            TotalPatients = patients.Count(),
            TodayAppointments = appointments.Count(a => a.Date.Date == DateTime.Today),
            RecentPatients = appointments
                .Where(a => a.Date.Date + a.Time <= DateTime.Now)
                .OrderByDescending(a => a.Date).ThenByDescending(a => a.Time)
                .DistinctBy(a => a.PatientId)
                .Take(3)
                .Select(a => new PatientDto
                {
                    Id = a.Patient.Id,
                    Name = a.Patient.User?.Name,
                    Email = a.Patient.User?.Email,
                    Conditions = a.Patient.Conditions.Select(c => c.Name).ToList()
                }).ToList(),
            UpcomingAppointments = appointments
                .Where(a => a.Date.Date + a.Time > DateTime.Now)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time)
                .Take(3)
                .Select(a => new AppointmentDto
                {
                    Id = a.Patient.Id,
                    PatientName = a.Patient.User?.Name,
                    Date = a.Date,
                    Time = a.Time.ToString(@"hh\:mm"),
                    Type = a.Type.ToString(),
                }).ToList()
        };

        return Ok(dashboardDto);
    }
}