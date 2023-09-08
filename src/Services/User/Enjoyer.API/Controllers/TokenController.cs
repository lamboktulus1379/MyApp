using System;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enjoyer.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IApplicationUserRepository _userService;
        private readonly ITokenService _tokenService;

        public TokenController(IApplicationUserRepository userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenApi tokenApi)
        {
            if (tokenApi is null)
            {
                return BadRequest("Invalid client request");
            }
            Console.WriteLine(tokenApi.RefreshToken, tokenApi.AccessToken);

            string accessToken = tokenApi.AccessToken;
            string refreshToken = tokenApi.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = _userService.GetApplicationUserByEmail(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _userService.Update(user.Id, user);

            return new ObjectResult(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("rovoke")]
        public IActionResult Revoke()
        {
            var email = User.Identity.Name;
            var user = _userService.GetApplicationUserByEmail(email);

            if (user == null)
                return BadRequest();

            user.RefreshToken = null;

            _userService.Update(user.Id, user);

            return NoContent();
        }
    }
}
