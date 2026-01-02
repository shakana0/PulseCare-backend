using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;
using Repositories.IRepositories;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    public record UserRequestDto(string Name, string Email);

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("sync")]
    [Authorize]
    public async Task<IActionResult> Sync(UserRequestDto request)
    {
        var clerkUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(clerkUserId))
        {
            return Unauthorized();
        }

        var exists = await _userRepository.ExistsAsync(clerkUserId);

        if (!exists)
        {
            var newUser = CreateUser(clerkUserId, request);
            await _userRepository.CreateAsync(newUser);
        }

        return NoContent();
    }

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
