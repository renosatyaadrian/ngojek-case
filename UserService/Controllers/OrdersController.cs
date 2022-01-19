using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrder _order;

        public OrdersController(IMapper mapper, IOrder order)
        {
            _mapper = mapper;
            _order = order;
        }

        [HttpPut("Accepted")]
        public async Task<ActionResult<OrderDto>> UpdateAcceptedOrder(UpdateAcceptedOrderDto updateAcceptedOrderDto)
        {
            try
            {
                var result = await _order.UpdateAcceptedOrder(updateAcceptedOrderDto);
                var dto = _mapper.Map<OrderDto>(result);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Completed")]
        public async Task<ActionResult<OrderDto>> UpdateCompletedOrder(UpdateCompletedOrderDto updateCompletedOrderDto)
        {
            try
            {
                var result = await _order.UpdateCompletedOrder(updateCompletedOrderDto);
                var dto = _mapper.Map<OrderDto>(result);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}