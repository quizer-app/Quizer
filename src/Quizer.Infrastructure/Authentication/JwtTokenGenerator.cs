﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quizer.Application.Common.Interfaces.Authentication;
using Quizer.Application.Common.Interfaces.Services;
using Quizer.Domain.UserAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quizer.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtSettings)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateAccessToken(User user)
    {
        SigningCredentials signingCredentials = CreateSigningCredentials();
        List<Claim> claims = CreateClaims(user);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateRefreshToken(User user)
    {
        SigningCredentials signingCredentials = CreateSigningCredentials();
        List<Claim> claims = CreateClaims(user);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private List<Claim> CreateClaims(User user)
    {
        return new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        };
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha512Signature
        );
    }
}
