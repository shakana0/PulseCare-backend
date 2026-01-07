using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Users;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var entities = await _noteRepository.GetAllByClerkUserIdAsync(userId);

        var response = entities.Select(n =>
        {
            var appointmentType = n.Appointment?.Type.ToString() ?? "";
            var date = n.Date.ToString("dd/MM/yyyy");

            return new NoteDto
            {
                Id = n.Id,
                Title = n.Title,
                Date = n.Date,
                DoctorName = n.Doctor?.User?.Name ?? "Unknown doctor",
                Content = n.Content,
                Diagnosis = n.Diagnosis,
                AppointmentDetails = $"From {appointmentType} appointment on {date}"
            };
        }).ToList();

        return Ok(response);
    }



}