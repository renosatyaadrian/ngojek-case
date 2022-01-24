using AdminService.Data;
using AdminService.Dtos;
using AdminService.Helper;
using AdminService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Registration([FromBody] AdminCreateDto adminCreateDto)
        {
            try
            {
                Console.WriteLine($"--> Register Admin <--");

                await _admin.Registration(adminCreateDto);
                await _admin.AddRoleForUser(adminCreateDto.Username, "Admin");

                return Ok($"Registrasi admin {adminCreateDto.Username} berhasil");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Authentication")]
        public async Task<ActionResult<User>> Authentication(LoginInput input)
        {
            try
            {
                Console.WriteLine($"--> Authentication Admin <--");

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
                Console.WriteLine($"--> Admin Get All Driver <--");

                var driver = await _admin.GetAllDriver();
                var dtos = _mapper.Map<IEnumerable<DriverDto>>(driver);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ApproveDriver")]
        public ActionResult ApproveDriver(int driverId)
        {
            try
            {
                Console.WriteLine($"--> Admin Accepting Driver Id : {driverId} <--");

                _admin.ApproveDriver(driverId);
                _admin.SaveChanges();

                return Ok("Driver Has Been Accepted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("BlockDriver")]
        public ActionResult BlockDriver(int driverId)
        {
            try
            {
                Console.WriteLine($"--> Admin Blocking Driver Id : {driverId}  <--");

                _admin.BlockDriver(driverId);
                _admin.SaveChanges();

                return Ok("Driver Has Been Blocked");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Customer
        [HttpGet("Customer")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomer()
        {
            try
            {
                Console.WriteLine($"--> Admin Get All Customer <--");

                var customer = await _admin.GetAllCustomer();
                var dtos = _mapper.Map<IEnumerable<CustomerDto>>(customer);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("BlockUser")]
        public ActionResult BlockCustomer(int customerId)
        {
            try
            {
                Console.WriteLine($"--> Admin Blocking Customer Id : {customerId}  <--");

                _admin.BlockCustomer(customerId);
                _admin.SaveChanges();

                return Ok("Customer Has Been Blocked");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UnlockUser")]
        public ActionResult UnlockCustomer(int customerId)
        {
            try
            {
                Console.WriteLine($"--> Admin Unblocking Customer Id : {customerId}  <--");

                _admin.UnblockCustomer(customerId);
                _admin.SaveChanges();

                return Ok("Customer Has Been Unblocked");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Transaction
        [HttpGet("Transaction")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetTransaction()
        {
            try
            {
                Console.WriteLine($"--> Admin Get All Transaction <--");

                var order = await _admin.GetAllTransaction();
                var dtos = _mapper.Map<IEnumerable<OrderDto>>(order);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPriceById")]
        public async Task<ActionResult<ConfigApp>> Get(int id)
        {
            Console.WriteLine($"--> Admin Get Price By Id : {id} <--");

            var result = await _admin.GetPriceById(id);
            if (result == null)
                return NotFound();
            return Ok(_mapper.Map<SetPriceDto>(result));
        }

        [HttpPost("PricePerKM")]
        public async Task<ActionResult<ConfigApp>> Post([FromBody] SetPriceCreateDto setPriceCreateDto)
        {
            try
            {
                Console.WriteLine($"--> Admin Set Price Per KM <--");

                var price = _mapper.Map<ConfigApp>(setPriceCreateDto);
                var result = await _admin.SetPricePerKM(price);
                var priceReturn = _mapper.Map<ConfigApp>(result);
                return Ok(priceReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PricePerKM")]
        public async Task<ActionResult<ConfigApp>> Put(int id, [FromBody] SetPriceCreateDto setPriceCreateDto)
        {
            try
            {
                Console.WriteLine($"--> Admin Update Price Per KM <--");

                var price = _mapper.Map<ConfigApp>(setPriceCreateDto);
                var result = await _admin.UpdatePricePerKM(id, price);
                var priceReturn = _mapper.Map<ConfigApp>(result);
                return Ok(priceReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
