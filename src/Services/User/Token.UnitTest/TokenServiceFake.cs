using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using TokenService;

namespace Token.UnitTest
{
    public class TokenServiceFake : ITokenService
    {
        public TokenServiceFake()
        {

        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<string> GenerateToken(User user)
        {
            throw new NotImplementedException();
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public Task<List<Claim>> GetClaims(User user)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }

        public SigningCredentials GetSigningCredentials()
        {
            throw new NotImplementedException();
        }

        public bool IsExpired(string token)
        {
            throw new NotImplementedException();
        }

        public Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            throw new NotImplementedException();
        }
    }
}
