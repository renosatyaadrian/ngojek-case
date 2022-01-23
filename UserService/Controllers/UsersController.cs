using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;
using UserService.SyncDataServices.Http;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IOrderDataClient _dataClient;
        private readonly IMapper _mapper;
        private readonly IUser _user;

        public UsersController(IMapper mapper, IUser user, IOrderDataClient dataClient)
        {
            _dataClient = dataClient;
            _mapper = mapper;
            _user = user;
        }

        [HttpPost("Authentication")]
        public async Task<ActionResult<User>> Login([FromBody] AuthenticateDto auth)
        {
            try
            {
                var user = await _user.Authenticate(auth.Username, auth.Password);
                if(user == null)
                    return BadRequest("Invalid username / password");
                return Ok(user);  
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");   
            }
        }

        [HttpPost("Roles")]
        public async Task<ActionResult> AddRole([FromBody] CreateRoleDto rolename)
        {
            try
            {
                await _user.AddRole(rolename.RoleName);
                return Ok($"Role {rolename.RoleName} berhasil ditambahkan");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("Order")]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _user.CreateOrder(createOrderDto);
                var orderDto = _mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<CustomerDto>> GetCustomerProfile()
        {
            var customer = await _user.GetUserProfile();
            var dtos = _mapper.Map<CustomerDto>(customer);
            return dtos;
        }

        [HttpGet("Roles")]
        public ActionResult<IEnumerable<CreateRoleDto>> GetAllRole()
        {
            return Ok(_user.GetAllRole());
        }

        [Authorize(Roles = "User")]
        [HttpGet("Order/{id}/OrderFee")]
        public async Task<ActionResult<OrderFeeDto>> GetOrderFee(int id)
        {
            try
            {
                var user = await _user.GetUserProfile();
                var order = _dataClient.GetUserOrderById(user.Id, id);
                var dtos = _mapper.Map<OrderFeeDto>(order.Result);
                return Ok(dtos); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderHistory()
        {
            try
            {
                var user = await _user.GetUserProfile();
                var orders = _dataClient.GetUserOrdersHistory(user.Id).Result;
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                 await _user.Registration(createUserDto);
                 return Ok($"Register user {createUserDto.Username} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");
            }
        }

        [HttpPut("Topup")]
        public async Task<ActionResult<CustomerBalanceDto>> TopupBalance(double amount)
        {
            try
            {
                var user = await _user.TopupBalance(amount);
                var balance = _mapper.Map<CustomerBalanceDto>(user);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error : {ex.Message}");
            }
        }
    }
}