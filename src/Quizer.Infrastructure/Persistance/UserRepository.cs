using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.Entities;

namespace Quizer.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly QuizerDbContext _context;

        public UserRepository(QuizerDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }
    }
}
