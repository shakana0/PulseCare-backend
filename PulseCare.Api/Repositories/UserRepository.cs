using PulseCare.API.Data.Entities.Users;
using Repositories.IRepositories;

namespace Repositories;

public class UserRepository : IUserRepository
{
    public Task CreateAsync(User request)
    {
        throw new NotImplementedException();
    }
}