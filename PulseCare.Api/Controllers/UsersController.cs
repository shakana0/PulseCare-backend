using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;
using PulseCare.API.Data.Enums;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("sync")]
    [Authorize]
    public async Task<IActionResult> Sync(UserRequestDto request)
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(clerkId))
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetUserAsync(clerkId);
        if (user == null)
        {
            user = CreateUser(clerkId, request);
            await _userRepository.AddUserAsync(user);
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole == "admin")
        {
            await EnsureDoctorUser(user);
        }
        else
        {
            await EnsurePatientUser(user);
        }

        return NoContent();
    }

    private async Task EnsureDoctorUser(User user)
    {
        if (await IsExistingDoctor(user))
        {
            return;
        }

        var patient = await _userRepository.GetPatientFromUserAsync(user.Id);
        if (patient != null)
        {
            await _userRepository.RemovePatientAsync(patient);
        }

        var newDoc = new Doctor { User = user, Specialty = "General" };
        user.Role = UserRoleType.Doctor;

        await _userRepository.AddDoctorAsync(newDoc);
    }


    private async Task EnsurePatientUser(User user)
    {
        if (await IsExistingUser(user))
        {
            return;
        }

        var newPatient = new Patient
        {
            User = user,
            CreatedAt = DateTime.UtcNow,
            DateOfBirth = DateTime.UtcNow.AddYears(-25)
        };

        await _userRepository.AddPatientAsync(newPatient);
    }

    private async Task<bool> IsExistingDoctor(User user) => await _userRepository.IsExistingDoctorAsync(user.Id);
    private async Task<bool> IsExistingUser(User user) => await _userRepository.IsExistingPatientAsync(user.ClerkId ?? "");

    private User CreateUser(string clerkUserId, UserRequestDto request)
    {
        var isDoctor = User.FindFirst(ClaimTypes.Role)?.Value == "admin";

        return new User
        {
            ClerkId = clerkUserId,
            Name = request.Name,
            Email = request.Email,
            Role = isDoctor ? UserRoleType.Doctor : UserRoleType.Patient
        };
    }
}
