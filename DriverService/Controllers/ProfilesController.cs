using AutoMapper;
using DriverService.Data;
using DriverService.Dtos;
using DriverService.Models;
using DriverService.SyncDataService.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDriverDataClient _driverDataClient;
        public ProfilesController(IDriverRepo repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IDriverDataClient driverDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _driverDataClient = driverDataClient;
        }

        [HttpGet("Profile")]
        public ActionResult<DriverDto> ShowProfile()
        {
            try
            {
                Console.WriteLine($"--> Getting Driver With UserName: {_repository.ShowProfile().Username} .....");
                var driveritem = _repository.ShowProfile();
                if (driveritem != null)
                {
                    return Ok(_mapper.Map<DriverDto>(driveritem));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"Bad Request: {ex.Message}");
            }
        }

        [HttpGet("Saldo")]
        public async Task<ActionResult<ReadSaldoDto>> GetSaldoDriver()
        {
            try
            {
                Console.WriteLine("--> Getting Saldo Driver .....");
                var orderitem = await _driverDataClient.GetSaldoDriver();
                if (orderitem != null)
                {
                    return Ok(orderitem);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Position")]
        public async Task<ActionResult<DriverDto>> SetPositionAsync(SetPositionDto setPositionDto)
        {
            try
            {
                Console.WriteLine($"--> Setting Driver Position.....");
                var drivermodel = _mapper.Map<Driver>(setPositionDto);

                if (drivermodel != null)
                {
                    try
                    {
                        _repository.SetPosition(drivermodel);
                        _repository.SaveChanges();

                        await _driverDataClient.SetPositionToOrderServicee(drivermodel);

                        return Ok($"Set Position Driver, Lat: {setPositionDto.DriverLatitude}, Long: {setPositionDto.DriverLongitude} Telah Berhasil");
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
                return BadRequest(ex.Message);
            }
        }
    }
}
