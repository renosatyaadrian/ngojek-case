using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        [HttpGet("Authentication")]
        public async Task<ActionResult<User>> Login(CreateUserDto auth)
        {
            try
            {
                var user = await _user.Authenticate(auth.Username, auth.Password);
                if(user == null)
                    return BadRequest("Invalid username / password");
                return Ok(user);  
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");   
            }
        }

        [HttpPost("Roles")]
        public async Task<ActionResult> AddRole([FromBody] CreateRoleDto rolename)
        {
            try
            {
                await _user.AddRole(rolename.RoleName);
                return Ok($"Role {rolename.RoleName} berhasil ditambahkan");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Roles")]
        public ActionResult<IEnumerable<CreateRoleDto>> GetAllRole()
        {
            return Ok(_user.GetAllRole());
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser(CreateUserDto createUserDto)
        {
            try
            {
                 await _user.Registration(createUserDto);
                 return Ok($"Register user {createUserDto.Username} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");
            }
        }
    }
}