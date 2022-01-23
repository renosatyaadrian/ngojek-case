using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUser _user;

        public UserController(IMapper mapper, IUser user)
        {
            _mapper = mapper;
            _user = user;
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}/Order/{orderId}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id, int orderId)
        {
            try
            {
                var order = await _user.GetOrderById(id, orderId);
                var dtos = _mapper.Map<OrderDto>(order);
                return Ok(dtos);  
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}/Orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderHistory(int id)
        {
            try
            {
                var orders = await _user.GetOrdersHistory(id);
                var dtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}