using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst("sub")?.Value
                     ?? connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId;
    }
}