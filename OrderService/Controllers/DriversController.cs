using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;
using System;
using System.Collections.Generic;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;

        public DriversController(IDriverRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("Driver")]
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

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [HttpPut("Position")]
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
                    return Ok($"Set Position Driver, Lat: {setPositionDto.DriverLatitude}, Long: {setPositionDto.DriverLongitude} Telah Berhasil");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [HttpGet("Order")]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrders()
        {
            try
            {
                Console.WriteLine("--> Getting Order .....");
                var orderitem = _repository.GetAllOrders();
                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orderitem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [HttpGet("History")]
        public ActionResult<IEnumerable<OrderDto>> GetHistoryOrder()
        {
            try
            {
                Console.WriteLine($"--> Getting History Driver .....");
                var orderitem = _repository.GetHistoryOrder();
                if (orderitem != null)
                {
                    return Ok(_mapper.Map<IEnumerable<OrderDto>>(orderitem));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [HttpPut("Accept")]
        public ActionResult AcceptOrder(CustIdDto custIdDto)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custIdDto.CustomerId}.....");

                _repository.AcceptOrder(custIdDto);
                _repository.SaveChanges();

                return Ok("Order Has Been Accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
        [HttpPut("Finish")]
        public ActionResult FinishOrder(CustIdDto custIdDto)
        {
            try
            {
                Console.WriteLine($"--> Driver Finishing Order With Customer Id: {custIdDto.CustomerId}.....");

                _repository.FinishOrder(custIdDto);
                _repository.SaveChanges();

                return Ok("Order Has Been Finished");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
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
    }
}
