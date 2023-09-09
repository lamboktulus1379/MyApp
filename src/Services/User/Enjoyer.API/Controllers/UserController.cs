using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Enjoyer.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserRepository _applicationUserService;
        private readonly IUserRepository _userService;
        private readonly IApplicationRoleRepository _roleService;
        private readonly UserManager<User> _userManager;
        private IMapper _mapper;
        private readonly ILoggerManager _logger;

        public UserController(IApplicationUserRepository service, IApplicationRoleRepository roleService, IMapper mapper, UserManager<User> userManager, IUserRepository userService, ILoggerManager logger)
        {
            _applicationUserService = service;
            _roleService = roleService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("/all")]
        //[HMACAuthentication]
        [Authorize(Roles = "Administrator,User")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAppicationUsers()
        {
            _logger.LogInfo("GetAppicationUsers");
            var applicationUsers = _applicationUserService.GetApplicationUsers();

            _logger.LogInfo(applicationUsers.ToString());
            return Ok(applicationUsers);
        }

        //[HttpGet("{id}")]
        //public ActionResult<ApplicationUser> Get(Guid id)
        //{
        //    var applicationUser = _applicationUserService.GetById(id);

        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(applicationUser);
        //}

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            UserDto userDto = _mapper.Map<UserDto>(user);
            if (userDto.Email == null)
            {
                userDto.Email = userDto.UserName;
            }

            return Ok(userDto);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetByEmaill(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ApplicationUserForCreation applicationUserRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUserEntity = _mapper.Map<ApplicationUser>(applicationUserRequest);

            var applicationUser = _applicationUserService.Add(applicationUserEntity);

            return CreatedAtAction("Get", new { id = applicationUser.Id }, applicationUser);
        }

        [HttpPatch]
        public ActionResult Patch([FromBody] ApplicationUserForRoles applicationUserForRolesRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated)
            {
                Console.WriteLine("Not Authenticated!");
                return Unauthorized();
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                Console.WriteLine("Not Authenticated!");
                return Unauthorized();
            }

            ApplicationUser applicationUser = _applicationUserService.PatchApplicationUserRoles(email, applicationUserForRolesRequest);

            return CreatedAtAction("Get", new { id = applicationUser.Id }, applicationUser);
        }

        [HttpDelete]
        public ActionResult Remove(Guid id)
        {
            var existingApplicationUser = _applicationUserService.GetById(id);

            if (existingApplicationUser == null)
            {
                return NotFound();
            }

            _applicationUserService.Delete(id);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            _logger.LogInfo("GetUsers");
            var users = await _userService.GetUsers();

            var usersDTO = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            string usersDTOString = JsonSerializer.Serialize(usersDTO);
            _logger.LogInfo(usersDTOString);
            return Ok(usersDTO);
        }

        [HttpPost("validate-user")]
        public ActionResult ValidateUser([FromBody] ValidateUser validateUser)
        {
            _logger.LogInfo($"ValidateUser: {validateUser}");
            return Ok(validateUser);
        }
    }
}
