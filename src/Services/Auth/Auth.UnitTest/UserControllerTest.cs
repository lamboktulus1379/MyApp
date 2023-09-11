using System.Collections.Generic;
using AutoMapper;
using Auth.API.Controllers;
using Auth.Core.Interfaces;
using Auth.Core.Models;
using Auth.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace User.UnitTest
{
    public class UserControllerTest
    {
        UserController _controller;
        IApplicationUserRepository _service;
        IUserRepository _userService;
        IRoleRepository _roleService;
        IMapper _mapper;
        UserManager<Enjoyer.Core.Models.User> userManager;
        ILoggerManager _logger;

        public UserControllerTest()
        {
            _userService = new UserServiceMock();
            _service = new UserServiceFake();
            _roleService = new RoleServiceFake();
            _logger = new LoggerManager();
            _controller = new UserController(_service, _roleService, _mapper, userManager, _userService, _logger);
        }
        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = _controller.GetAppicationUsers();

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsAllUsers()
        {
            // Act
            var okResult = _controller.GetAppicationUsers().Result as OkObjectResult;

            // Assert
            var aplicationsUsers = Assert.IsType<List<ApplicationUser>>(okResult.Value);
            Assert.Equal(3, aplicationsUsers.Count);
        }

    }
}
