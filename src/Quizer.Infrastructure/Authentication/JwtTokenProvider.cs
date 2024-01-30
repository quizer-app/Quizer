using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Services;
using Quizer.Domain.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quizer.Infrastructure.Authentication;

public class JwtTokenProvider : IJwtTokenProvider
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<User> _userManager;

    public JwtTokenProvider(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettings, UserManager<User> userManager)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateAccessToken(User user)
    {
        SigningCredentials signingCredentials = CreateSigningCredentials();
        List<Claim> claims = await CreateClaims(user);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateAccessToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(refreshToken);

        SigningCredentials signingCredentials = CreateSigningCredentials();
        List<Claim> claims = jwtToken.Claims.ToList();

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddDays(_jwtSettings.AccessTokenExpiryMinutes)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public async Task<string> GenerateRefreshToken(User user)
    {
        SigningCredentials signingCredentials = CreateSigningCredentials();
        List<Claim> claims = await CreateClaims(user);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public async Task<bool> ValidateRefreshToken(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var result = await tokenHandler.ValidateTokenAsync(refreshToken, validationParameters);
        return result.IsValid;
    }

    private async Task<List<Claim>> CreateClaims(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.GivenName, user.UserName!),
            new (JwtRegisteredClaimNames.Email, user.Email!),
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha512Signature
        );
    }
}
