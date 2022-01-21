using AdminService.Data;
using AdminService.Dtos;
using AdminService.Helper;
using AdminService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;
        private IMapper _mapper;
        private IAdmin _admin;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IAdmin admin, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _admin = admin;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSettings.Value;

        }

        [HttpPost]
        public async Task<ActionResult> Registration([FromBody] AdminCreateDto adminCreateDto)
        {
            try
            {
                await _admin.Registration(adminCreateDto);
                await _admin.AddRoleForUser(adminCreateDto.Username, "Admin");

                return Ok($"Registrasi admin {adminCreateDto.Username} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Authentication(LoginInput input)
        {
            try
            {
                var user = await _admin.Authenticate(input.Username, input.Password);
                if (user == null) return BadRequest("username/password tidak tepat");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        //Driver
        [HttpGet("Driver")]
        public async Task<ActionResult<IEnumerable<DriverDto>>> GetDriver()
        {
            try
            {
                var customer = await _admin.GetAllDriver();
                return Ok(customer);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        //Customer
        [HttpGet("Customer")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomer()
        {
            try
            {
                var customer = await _admin.GetAllCustomer();
                return Ok(customer);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        //Transaction
        [HttpGet("Transaction")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetTransaction()
        {
            try
            {
                var customer = await _admin.GetAllTransaction();
                return Ok(customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
