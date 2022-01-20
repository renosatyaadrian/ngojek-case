using AutoMapper;
using DriverService.Data;
using DriverService.Dtos;
using DriverService.Models;
using DriverService.SyncDataService.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationsController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private readonly IDriverDataClient _driverDataClient;
        private readonly IMapper _mapper;

        public AdministrationsController(IDriverRepo repository, IDriverDataClient driverDataClient, IMapper mapper)
        {
            _repository = repository;
            _driverDataClient = driverDataClient;
            _mapper = mapper;
            _driverDataClient = driverDataClient;
        }

        [HttpPost("Regitration")]
        [AllowAnonymous]
        public async Task<ActionResult> Registration(DriverForCreateDto driverForCreateDto)
        {
            try
            {
                Console.WriteLine($"--> User Registration With Username: {driverForCreateDto.Username} .....");
                await _repository.Registration(driverForCreateDto);

                var httpcontent = _mapper.Map<DriverForSendHttpDto>(driverForCreateDto);

                if (driverForCreateDto != null)
                {
                    try
                    {
                        await _driverDataClient.SendDriverToOrderService(httpcontent);
                        return Ok($"Registrasi Driver: {driverForCreateDto.Username} Telah Berhasil");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could Not Send Synchronously: {ex.Message}");
                    }
                }
                return NotFound();
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
