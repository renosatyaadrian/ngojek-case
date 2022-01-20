using AutoMapper;
using DriverService.Data;
using DriverService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Driver")]
    public class OrdersController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(IDriverRepo repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrders()
        {
            try
            {
                Console.WriteLine("--> Getting Order .....");
                var orderitem = _repository.GetAllOrders();
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

        [HttpGet("History")]
        public ActionResult<OrderDto> GetHistoryOrder()
        {
            try
            {
                var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;

                Console.WriteLine($"--> Getting History Driver With Username: {userName} .....");
                var orderitem = _repository.GetHistoryOrder();
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

        [HttpPut("AcceptOrder")]
        public ActionResult AcceptOrder(int custId)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custId}.....");

                _repository.AcceptOrder(custId);

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

                return Ok("Order Has Been Finished");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
