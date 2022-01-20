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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class ProfilesController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private readonly IDriverDataClient _driverDataClient;
        private readonly IMapper _mapper;

        public ProfilesController(IDriverRepo repository, IDriverDataClient driverDataClient, IMapper mapper)
        {
            _repository = repository;
            _driverDataClient = driverDataClient;
            _mapper = mapper;
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
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Saldo")]
        public ActionResult<ReadSaldoDto> ShowSaldo()
        {
            try
            {
                Console.WriteLine($"--> Getting Balance Driver: {_repository.ShowSaldo().Balance} .....");
                var driveritem = _repository.ShowSaldo();
                if (driveritem != null)
                {
                    return Ok(_mapper.Map<ReadSaldoDto>(driveritem));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
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
                        await _driverDataClient.SetPositionToOrderServicee(setPositionDto);

                        _repository.SetPosition(drivermodel);
                        _repository.SaveChanges();

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
