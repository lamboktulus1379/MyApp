﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Enjoyer.Core.DataTransferObjects;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Enjoyer.Usecase.UserUsecase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<UserController> _logger;
        private readonly IUserUsecase _userUsecase;

        public UserController(IApplicationUserRepository service, IApplicationRoleRepository roleService, IMapper mapper, UserManager<User> userManager, IUserRepository userService, ILogger<UserController> logger, IUserUsecase userUsecase)
        {
            _applicationUserService = service;
            _roleService = roleService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
            _logger = logger;
            _userUsecase = userUsecase;
        }

        [HttpGet("/all")]
        //[HMACAuthentication]
        [Authorize(Roles = "Administrator,User")]
        public ActionResult<IEnumerable<ApplicationUser>> GetAppicationUsers()
        {
            _logger.LogInformation("GetAppicationUsers");
            var applicationUsers = _applicationUserService.GetApplicationUsers();

            _logger.LogInformation(applicationUsers.ToString());
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
            var response = new Res
            {
                ResponseMessage = "OK",
                ResponseCode = "200",
            };
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
            userDto.Roles = _userManager.GetRolesAsync(user).Result.ToList();

            response.Data = userDto;
            return Ok(response);
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
            var response = new Res
            {
                ResponseMessage = "OK",
                ResponseCode = "200",
            };

            _logger.LogInformation("GetUsers");
            var users = await _userService.GetUsers();

            string userstring = JsonSerializer.Serialize(users);
            _logger.LogInformation(userstring);

            response.Data = users;
            return Ok(response
            );
        }

        [HttpPost("validate-user")]
        public ActionResult ValidateUser([FromBody] ValidateUser validateUser)
        {
            _logger.LogInformation($"ValidateUser: {validateUser}");
            return Ok(validateUser);
        }

        [HttpPut("balance")]
        public ActionResult UpdateBalance([FromBody] UserBalanceForCreation userBalanceForCreation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _userUsecase.UpdateBalance(userBalanceForCreation);

            return NoContent();
        }
    }
}
