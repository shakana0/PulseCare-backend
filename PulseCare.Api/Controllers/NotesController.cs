using System.Security.Claims;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;

    public NotesController(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NoteDto>>> GetAll()
    {
        var userId = await GetUserIdAsync();

        if (userId == null)
        {
            return NotFound("User not found");
        }

        var entities = await _noteRepository.GetAllByIdAsync(userId);

        var response = entities.Select(n =>
        {
            var appointmentType = n.Appointment?.Type.ToString() ?? "";
            var date = n.Date.ToString("dd/MM/yyyy");

            return new NoteDto
            {
                NoteId = n.Id,
                Title = n.Title,
                CreatedAt = n.Date,
                AuthorName = n.Doctor?.User?.Name ?? "Unknown doctor",
                Content = n.Content,
                Diagnosis = n.Diagnosis,
                AppointmentDetails = $"From {appointmentType} appointment on {date}"
            };
        }).ToList();

        return Ok(response);
    }

    private async Task<Guid?> GetUserIdAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return null;

        if (!Guid.TryParse(userId, out Guid result))
        {
            return null;
        }

        return result;
    }

}