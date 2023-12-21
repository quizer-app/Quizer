using Quizer.Domain.UserAggregate;

namespace Quizer.Application.Common.Interfaces.Persistance
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
