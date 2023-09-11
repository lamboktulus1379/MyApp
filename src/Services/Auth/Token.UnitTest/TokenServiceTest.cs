using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TokenService;
using Xunit;

namespace Token.UnitTest
{
    public class TokenServiceTest
    {
        UserManager<User> _userManager;
        ITokenService _tokenService;
        public TokenServiceTest()
        {
            _tokenService = new TokenService.TokenService(_userManager);
        }

        [Fact]
        public void Get_WhenCalled_ReturnPrincipalResult()
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJsYW1ib2suc2ltYW1vcmFAbXlncmEudGVjaCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJleHAiOjE2Mzk4MTYxODUsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMS8saHR0cDovL2xvY2FsaG9zdDo1MDAzLyxodHRwOi8vbG9jYWxob3N0OjUwMDUvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAxLyJ9.OXGbP2NQUo9xrMbaY3INxIoGrU1a2yoLnWGNT7p5TjA";
            ClaimsPrincipal claimPrincipal = _tokenService.GetPrincipalFromExpiredToken(token);
            Assert.NotNull(claimPrincipal);
        }

    }
}
