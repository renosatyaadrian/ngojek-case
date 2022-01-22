using AutoMapper;
using DriverService.Data;
using DriverService.Dtos;
using DriverService.SyncDataService.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DriverService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Driver")]
    public class OrdersController : ControllerBase
    {
        private readonly IDriverRepo _repository;
        private IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDriverDataClient _driverDataClient;

        public OrdersController(IDriverRepo repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IDriverDataClient driverDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _driverDataClient = driverDataClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            try
            {
                Console.WriteLine("--> Getting Order .....");
                var orderitem = await _driverDataClient.GetOrderFromOrderService();
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
        public async Task<ActionResult<OrderDto>> GetHistoryOrder()
        {
            try
            {
                Console.WriteLine("--> Getting History Order .....");
                var orderitem = await _driverDataClient.GetHistoryOrderFromOrderService();
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
        public ActionResult AcceptOrder(CustIdDto custIdDto)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custIdDto.CustomerId}.....");

                _repository.AcceptOrder(custIdDto);

                return Ok("Order Has Been Accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("FinishOrder")]
        public ActionResult FinishOrder(CustIdDto custIdDto)
        {
            try
            {
                Console.WriteLine($"--> Driver Accepting Order With Customer Id: {custIdDto.CustomerId}.....");

                _repository.FinishOrder(custIdDto);

                return Ok("Order Has Been Finished");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
