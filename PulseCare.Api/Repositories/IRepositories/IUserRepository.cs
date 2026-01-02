using PulseCare.API.Data.Entities.Users;

namespace Repositories.IRepositories;

public interface IUserRepository
{
    Task CreateAsync(User request);
}