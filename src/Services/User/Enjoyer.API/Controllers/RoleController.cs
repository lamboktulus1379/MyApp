using System;
using System.Collections.Generic;
using AutoMapper;
using Enjoyer.Core.Interfaces;
using Enjoyer.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Enjoyer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IApplicationRoleRepository _roleService;
        private IMapper _mapper;

        public RoleController(IApplicationRoleRepository service, IMapper mapper)
        {
            _roleService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            var roles = _roleService.GetApplicationRoles();

            return Ok(roles);
        }

        [HttpGet("{id}", Name = "Get")]
        public ActionResult<ApplicationRole> Get(Guid id)
        {
            var role = _roleService.GetById(id);

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public ActionResult Post([FromBody] RoleForCreation roleRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleByName = _roleService.GetByName(roleRequest.Name);

            if (roleByName != null)
            {
                // TODO: Handle Response Conflict 
                return Conflict(roleByName);
            }

            var roleEntity = _mapper.Map<ApplicationRole>(roleRequest);

            var role = _roleService.Add(roleEntity);

            return CreatedAtAction("Get", new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, [FromBody] RoleForUpdate roleForUpdate)
        {
            var existingRole = _roleService.GetById(id);

            if (existingRole == null)
            {
                return NotFound();
            }

            var roleEntity = _mapper.Map<ApplicationRole>(roleForUpdate);

            var role = _roleService.Update(id, roleEntity);
            return Ok(role);
        }

        [HttpDelete]
        public ActionResult Remove(Guid id)
        {
            var existingRole = _roleService.GetById(id);

            if (existingRole == null)
            {
                return NotFound();
            }

            _roleService.Delete(id);
            return Ok();
        }
    }
}
