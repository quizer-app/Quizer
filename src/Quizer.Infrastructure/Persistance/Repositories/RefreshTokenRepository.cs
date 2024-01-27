using Microsoft.EntityFrameworkCore;
using Quizer.Application.Common.Interfaces.Persistance;
using Quizer.Domain.RefreshTokenAggregate;

namespace Quizer.Infrastructure.Persistance.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly QuizerDbContext _context;

    public RefreshTokenRepository(QuizerDbContext context)
    {
        _context = context;
    }

    public async Task Add(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }

    public void Delete(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
    }

    public async Task<RefreshToken?> Get(TokenId id)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Id == id);
    }
}