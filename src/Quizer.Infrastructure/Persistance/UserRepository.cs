using Microsoft.EntityFrameworkCore;
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

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
