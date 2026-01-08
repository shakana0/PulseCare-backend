using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PulseCare.API.Data.Enums;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    private readonly IHubContext<PulseCareHub> _hub;


    public ConversationsController(IConversationRepository conversationRepository, IMessageRepository messageRepository, IUserRepository userRepository, IHubContext<PulseCareHub> hub)
    {
        _conversationRepository = conversationRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _hub = hub;
    }


    [HttpGet]
    public async Task<ActionResult<List<ConversationDto>>> GetConversations()
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(clerkId)) return Unauthorized();

        var user = await _userRepository.GetUserAsync(clerkId);
        if (user == null) return NotFound("User not found in local database.");

        Guid profileId = Guid.Empty;

        if (user.Role == UserRoleType.Patient)
        {
            var patient = await _userRepository.GetPatientFromUserAsync(user.Id);
            if (patient == null) return BadRequest("Patient not found.");
            profileId = patient.Id;
        }
        else if (user.Role == UserRoleType.Doctor)
        {
            var doctor = await _userRepository.GetDoctorFromUserAsync(user.Id);
            if (doctor == null) return BadRequest("Doctor not found");
            profileId = doctor.Id;
        }

        var conversations = user.Role switch
        {
            UserRoleType.Patient => await _conversationRepository.GetConversationsForPatientAsync(profileId),
            UserRoleType.Doctor => await _conversationRepository.GetConversationsForDoctorAsync(profileId),
            _ => null
        };

        if (conversations == null) return Ok(new List<ConversationDto>());

        var dtos = conversations.Select(conv =>
        {
            var latest = conv.Messages.OrderByDescending(m => m.Date).FirstOrDefault();
            var latestDto = latest is null ? null : new MessageDto(
                latest.Id, latest.Subject, latest.Content, latest.Date, latest.Read, latest.FromPatient, latest.ConversationId
            );

            var unread = user.Role switch
            {
                UserRoleType.Patient => conv.Messages.Count(m => !m.FromPatient && !m.Read),
                UserRoleType.Doctor => conv.Messages.Count(m => m.FromPatient && !m.Read),
                _ => 0
            };

            return new ConversationDto(conv.Id, conv.PatientId, conv.DoctorId, latestDto, unread);
        }).ToList();

        return Ok(dtos);
    }

    [HttpPost("start")]
    [Authorize]
    public async Task<IActionResult> StartConversation([FromBody] StartConversationRequest request)
    {
        var clerkId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(clerkId)) return Unauthorized();

        var currentUser = await _userRepository.GetUserAsync(clerkId);
        if (currentUser == null) return NotFound("User not found in database.");

        Guid finalPatientId;
        Guid finalDoctorId;

        if (currentUser.Role == UserRoleType.Patient)
        {
            var patientProfile = await _userRepository.GetPatientFromUserAsync(currentUser.Id);
            if (patientProfile == null) return BadRequest("Patient profile missing.");
            finalPatientId = patientProfile.Id;

            if (Guid.TryParse(request.DoctorId, out Guid docGuid))
            {
                finalDoctorId = docGuid;
            }
            else
            {
                var docUser = await _userRepository.GetUserAsync(request.DoctorId!);
                if (docUser == null) return BadRequest("Target doctor user not found.");

                var docProfile = await _userRepository.GetDoctorFromUserAsync(docUser.Id);
                if (docProfile == null) return BadRequest("Target doctor profile missing.");
                finalDoctorId = docProfile.Id;
            }
        }
        else if (currentUser.Role == UserRoleType.Doctor)
        {
            var doctorProfile = await _userRepository.GetDoctorFromUserAsync(currentUser.Id);
            if (doctorProfile == null) return BadRequest("Doctor profile missing.");
            finalDoctorId = doctorProfile.Id;

            if (Guid.TryParse(request.PatientId, out Guid patGuid))
            {
                finalPatientId = patGuid;
            }
            else
            {
                var targetPatientUser = await _userRepository.GetUserAsync(request.PatientId!);
                if (targetPatientUser == null) return BadRequest("Target patient user not found.");

                var targetPatientProfile = await _userRepository.GetPatientFromUserAsync(targetPatientUser.Id);
                if (targetPatientProfile == null) return BadRequest("Target patient profile missing.");
                finalPatientId = targetPatientProfile.Id;
            }
        }
        else
        {
            return BadRequest("Unsupported role.");
        }

        var conversation = await _conversationRepository.GetOrCreateForPatientAndDoctorAsync(finalPatientId, finalDoctorId);

        var messageEntity = await _messageRepository.CreateMessageAsync(
            conversation.Id,
            request.Subject,
            request.Content,
            request.FromPatient
        );

        var dto = new MessageDto(
            messageEntity.Id,
            messageEntity.Subject,
            messageEntity.Content,
            messageEntity.Date,
            messageEntity.Read,
            messageEntity.FromPatient,
            messageEntity.ConversationId
        );

        await _hub.Clients.Group(conversation.Id.ToString()).SendAsync("ReceiveMessage", dto);

        return Ok(new StartConversationResponse(conversation.Id, dto));
    }

}
