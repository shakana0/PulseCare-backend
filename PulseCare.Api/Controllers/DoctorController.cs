using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DoctorController : ControllerBase
{

    private readonly IDoctorRepository _doctorRepository;

    public DoctorController(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }


    [HttpGet]
    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllDoctorsAsync();

        var dtos = doctors.Select(d => new DoctorDto(
            d.Id,
            d.UserId,
            d.User?.Name ?? "",
            d.User?.Email ?? ""
        ));

        return dtos;
    }
}