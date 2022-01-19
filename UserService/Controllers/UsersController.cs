using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUser _user;

        public UsersController(IMapper mapper, IUser user)
        {
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