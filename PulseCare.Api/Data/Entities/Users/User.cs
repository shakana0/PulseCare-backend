using PulseCare.API.Data.Enums;

namespace PulseCare.API.Data.Entities.Users;
 public class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public UserRoleType Role { get; set; }
    public string? Avatar { get; set; }
}
