using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Auth.Core.DataTransferObjects;
using Auth.Core.Models;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public bool IsExpired(string token);
        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth);
        public Task<string> GenerateToken(User user);
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        public Task<List<Claim>> GetClaims(User user);
        public SigningCredentials GetSigningCredentials();
    }
}
