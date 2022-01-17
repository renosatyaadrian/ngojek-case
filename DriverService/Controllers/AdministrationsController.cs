using DriverService.Data;
using DriverService.Dtos;
using DriverService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationsController : ControllerBase
    {
        private readonly IDriverRepo _repository;

        public AdministrationsController(IDriverRepo repository)
        {
            _repository = repository;
        }

        [HttpPost("Regitration")]
        [AllowAnonymous]
        public async Task<ActionResult> Registration(DriverForCreateDto driverForCreateDto)
        {
            try
            {
                Console.WriteLine($"--> User Registration With Username: {driverForCreateDto.Username} .....");
                await _repository.Registration(driverForCreateDto);
                return Ok($"Registrasi User: {driverForCreateDto.Username} Telah Berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("Authentication")]
        [AllowAnonymous]
        public async Task<ActionResult<Driver>> Login(UserForCreateDto userForCreateDto)
        {
            try
            {
                Console.WriteLine($"--> User Login With Username: {userForCreateDto.Username} .....");
                var user = await _repository.Login(userForCreateDto.Username, userForCreateDto.Password);
                if (user == null)
                {
                    return BadRequest("Username / Password Tidak Tepat");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
