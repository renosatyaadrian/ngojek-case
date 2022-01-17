using AutoMapper;
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
    [Authorize(Roles = "Driver")]
    public class ProfilesController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;

        public ProfilesController(IDriverRepo repository, IMapper mapper)
        {
            _repository = repository;
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

        [HttpPost]
        public ActionResult<DriverDto> CreateDriver(DriverForCreateDto driverForCreateDto)
        {
            try
            {
                Console.WriteLine("--> Creating Driver .....");
                var drivermodel = _mapper.Map<Driver>(driverForCreateDto);
                _repository.CreateDriver(drivermodel);
                _repository.SaveChanges();

                var driverReadDto = _mapper.Map<DriverDto>(drivermodel);

                if (driverReadDto != null)
                {
                    try
                    {
                        return Ok(driverReadDto);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could Not Send Message: {ex.Message}");
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<DriverDto> SetPosition(SetPositionDto setPositionDto)
        {
            try
            {
                Console.WriteLine($"--> Setting Driver Position.....");
                var drivermodel = _mapper.Map<Driver>(setPositionDto);
                _repository.SetPosition(drivermodel);
                _repository.SaveChanges();

                if (drivermodel != null)
                {
                    return Ok(drivermodel);
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
