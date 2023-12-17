using Quizer.Domain.Entities;

namespace Quizer.Application.Common.Interfaces.Persistance
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetUserByEmail(string email);
    }
}
