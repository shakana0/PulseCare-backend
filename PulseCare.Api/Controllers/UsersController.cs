using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;

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

        var userRole = User.FindFirst(ClaimTypes.Role);
        if (userRole?.Value == "admin")
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

        await _userRepository.AddDoctorAsync(newDoc);
    }


    private async Task EnsurePatientUser(User user)
    {
        if (await IsExistingUser(user))
        {
            return;
        }

        var newPatient = new Patient { User = user };

        await _userRepository.AddPatientAsync(newPatient);
    }

    private async Task<bool> IsExistingDoctor(User user) => await _userRepository.IsExistingDoctorAsync(user.Id);
    private async Task<bool> IsExistingUser(User user) => await _userRepository.IsExistingPatientAsync(user.ClerkId ?? "");

    private User CreateUser(string clerkUserId, UserRequestDto request)
    {

        return new User
        {
            ClerkId = clerkUserId,
            Name = request.Name,
            Email = request.Email
        };
    }
}
