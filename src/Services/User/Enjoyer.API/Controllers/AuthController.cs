using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Enjoyer.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly IApplicationUserRepository _applicationUserService;
        private readonly IApplicationRoleRepository _roleService;
        private readonly IHashing _hashing;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IApplicationUserRepository applicationUserService, IUserRepository userService, ITokenService tokenService, IMapper mapper, IApplicationRoleRepository roleService, IHashing hashing, UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleService = roleService;
            _hashing = hashing;
            _userManager = userManager;
            _applicationUserService = applicationUserService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult GetUser()
        {
            var principal = new ClaimsPrincipal();
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var roles = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"Email is: {email}, Roles is: {roles}");

            var user = _userManager.FindByEmailAsync(email);

            var userForSelect = _mapper.Map<UserForSelect>(user);

            return Ok(userForSelect);
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Login dbUser)
        {
            _logger.LogInformation("Login");
            if (dbUser == null)
            {
                return BadRequest("Invalid client request");
            }

            ApplicationUser user = _applicationUserService.GetApplicationUserByEmail(dbUser.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            _logger.LogInformation($"User Login is {user.FirstName}");

            dbUser.Password = _hashing.Generate(dbUser.Password);
            if (!user.Password.Equals(dbUser.Password))
            {
                return Unauthorized();
            }
            var applicationUser = _applicationUserService.GetApplicationUserAndRolesByEmail(user.Email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, dbUser.Email)
            };

            foreach (var role in applicationUser.ApplicationRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);

            _applicationUserService.Update(user.Id, user);

            return Ok(new
            {
                Id = user.Id,
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost, Route("login-identity")]
        public async Task<IActionResult> LoginIdentity([FromBody] Login dbUser)
        {
            _logger.LogInformation("LoginIdentity");
            var user = await _userManager.FindByNameAsync(dbUser.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dbUser.Password))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            _logger.LogInformation($"User Login is {user.FirstName}");
            var signingCredentials = _tokenService.GetSigningCredentials();
            var claims = await _tokenService.GetClaims(user);
            var tokenOptions = _tokenService.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost, Route("register")]
        public IActionResult Register([FromBody] ApplicationUserDto user)
        {
            // Handle Transaction
            var candidate = _userService.GetUserByEmail(user.Email);
            if (candidate != null)
            {
                return StatusCode(409, new { title = "Email already registered" });
            }

            if (!ModelState.IsValid)
            {
                //_logger.LogError("Invalid user object sent from client.");
                return BadRequest("Invalid model object");
            }

            user.Password = _hashing.Generate(user.Password);

            var userEntity = _mapper.Map<ApplicationUser>(user);
            ApplicationRole role = _roleService.GetByName("User");

            if (role != null)
            {
                List<ApplicationRole> roles = new List<ApplicationRole>();
                roles.Add(role);
                userEntity.ApplicationRoles = roles;
            }
            _applicationUserService.Add(userEntity);

            return NoContent();
        }

        [HttpPost, Route("register-identity")]
        public async Task<IActionResult> RegisterIdentity([FromBody] UserRegistrationModel userModel)
        {
            var candidate = _userService.GetUserByEmail(userModel.Email);
            if (candidate != null)
            {
                return StatusCode(409, new { title = "Email already registered" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = _mapper.Map<User>(userModel);

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "Viewer");

            return NoContent();
        }

        [HttpPost, Route("externallogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _tokenService.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");
            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User { Email = payload.Email, UserName = payload.Email, CreatedAt = DateTime.Now, FirstName = payload.Name, LastName = payload.FamilyName };

                    var identityUser = await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }


            }
            if (user == null)
            {
                _logger.LogInformation($"Invalid External Authentication.");
                return BadRequest("Invalid External Authentication.");
            }
            var token = await _tokenService.GenerateToken(user);
            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(5);

            user = _userService.Update(user.Id, user);
            _logger.LogInformation($"Updated user: {user}");
            //check for the Locked out account
            AuthResponseDto auth = new AuthResponseDto { Token = token, IsAuthSuccessful = true, RefreshToken = user.RefreshToken };
            string authString = JsonConvert.SerializeObject(auth);
            _logger.LogInformation($"Token generated: {authString}");

            return Ok(auth);
        }

        [HttpPost, Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] ReqRefreshToken refreshToken)
        {
            _logger.LogInformation("RefreshToken");
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Token is empty");
                return BadRequest("Please provide refresh token");
            }


            var principal = _tokenService.GetPrincipalFromExpiredToken(refreshToken.AccessToken);
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            User user = await _userManager.FindByEmailAsync(email);
            if (user is null || user.RefreshToken != refreshToken.RefreshToken
                )
            {
                _logger.LogInformation("User not found");
                return NotFound("User not found");
            }
            if (user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogInformation("Token Expired");
                return Unauthorized("Token Expired");
            }

            string newToken = await _tokenService.GenerateToken(user);
            string newRefreshToken = _tokenService.GenerateRefreshToken();
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(2);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;

            user = _userService.Update(user.Id, user);
            AuthResponseDto auth = new AuthResponseDto { Token = newToken, IsAuthSuccessful = true, RefreshToken = user.RefreshToken };
            string authString = JsonConvert.SerializeObject(auth);
            _logger.LogInformation($"Token generated: {authString}");

            return Ok(auth);
        }
    }
}
