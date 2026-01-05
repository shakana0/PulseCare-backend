using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminDashboardController(IPatientRepository patientRepository) : ControllerBase
{
    private readonly IPatientRepository _patientRepository = patientRepository;

    // GET: /{patientId}/dashboard
    [HttpGet]
    public async Task<ActionResult<AdminDashboardDTO>> GetAdminDashboard()
    {
        var patients = await _patientRepository.GetAllPatientsAsync();
        
    }
}