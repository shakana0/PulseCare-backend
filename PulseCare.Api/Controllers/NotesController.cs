using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseCare.API.Data.Entities.Communication;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly INoteRepository _noteRepository;
    private readonly IUserRepository _userRepository;

    public NotesController(INoteRepository noteRepository, IUserRepository userRepository)
    {
        _noteRepository = noteRepository;
        _userRepository = userRepository;
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

    [HttpPost]
    public async Task<ActionResult> AddNote(CreateNoteDto request)
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(clerkId))
        {
            return Unauthorized();
        }

        var doctor = await _userRepository.GetDoctorWithClerkIdAsync(clerkId);
        var note = new Note
        {
            AppointmentId = Guid.Parse(request.AppointmentId),
            PatientId = Guid.Parse(request.PatientId),
            Doctor = doctor,
            Title = request.Title,
            Diagnosis = request.Diagnosis,
            Content = request.Content,
            Date = DateTime.Now
        };

        var success = await _noteRepository.AddNoteAsync(note);
        if (!success)
        {
            return BadRequest();
        }

        return NoContent();
    }
}