using AutoMapper;
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
                    return Ok(drivermodel);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReadOrderDto>> GetAllOrders()
        {
            try
            {
                Console.WriteLine("--> Getting Enrollments .....");
                var orderitem = _repository.GetAllOrders();
                return Ok(_mapper.Map<IEnumerable<ReadOrderDto>>(orderitem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("History")]
        public ActionResult<ReadOrderDto> GetHistoryOrder()
        {
            try
            {
                Console.WriteLine($"--> Getting History Driver With Id: {_repository.GetHistoryOrder().DriverId} .....");
                var orderitem = _repository.GetHistoryOrder();
                if (orderitem != null)
                {
                    return Ok(_mapper.Map<ReadOrderDto>(orderitem));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Accept")]
        public ActionResult AcceptOrder(int custId)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custId}.....");

                _repository.AcceptOrder(custId);
                _repository.SaveChanges();

                return Ok("Order Has Been Accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Finish")]
        public ActionResult FinishOrder(int custId)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custId}.....");

                _repository.FinishOrder(custId);
                _repository.SaveChanges();

                return Ok("Order Has Been Finished");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
