using AutoMapper;
using DriverService.Data;
using DriverService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class OrdersController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;

        public OrdersController(IDriverRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrders()
        {
            try
            {
                Console.WriteLine("--> Getting Enrollments .....");
                var orderitem = _repository.GetAllOrders();
                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orderitem));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("History")]
        public ActionResult<OrderDto> GetHistoryOrder()
        {
            try
            {
                Console.WriteLine($"--> Getting History Driver With Id: {_repository.GetHistoryOrder().DriverId} .....");
                var orderitem = _repository.GetHistoryOrder();
                if (orderitem != null)
                {
                    return Ok(_mapper.Map<OrderDto>(orderitem));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AcceptOrder")]
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

        [HttpPut("FinishOrder")]
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
