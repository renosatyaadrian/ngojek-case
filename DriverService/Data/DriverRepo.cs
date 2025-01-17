﻿using DriverService.Dtos;
using DriverService.Helper;
using DriverService.Models;
using DriverService.SyncDataService.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DriverService.Data
{
    public class DriverRepo : IDriverRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;
        private readonly IDriverDataClient _driverDataClient;
        public DriverRepo(ApplicationDbContext context, 
                          IHttpContextAccessor httpContextAccessor, 
                          UserManager<IdentityUser> userManager, 
                          RoleManager<IdentityRole> roleManager, 
                          IOptions<AppSettings> appSetings,
                          IDriverDataClient driverDataClient)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSetings.Value;
            _driverDataClient = driverDataClient;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        //Administration
        public async Task<User> Login(string username, string password)
        {
            try
            {
                var userFind = await _userManager.CheckPasswordAsync(await _userManager.FindByNameAsync(username), password);
                if (!userFind)
                {
                    throw new Exception("Mohon login kembali");
                }

                var driver = _context.Drivers.FirstOrDefault(dri => dri.Username == username);

                if(driver.Blocked.Equals(false))
                {
                    var user = new User
                    {
                        Username = username
                    };

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    var roles = await GetRolesFromUser(username);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.UtcNow.AddHours(3),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    user.Token = tokenHandler.WriteToken(token);
                    return user;
                }
                else
                {
                    throw new Exception("Mohon tunggu sampai admin memvalidasi akun anda");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task Registration(DriverForCreateDto driverForCreateDto)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync("Driver");
                if (!roleExist.Equals(false))
                {
                    var newUser = new IdentityUser
                    {
                        UserName = driverForCreateDto.Username,
                        Email = driverForCreateDto.Username

                    };

                    var result = await _userManager.CreateAsync(newUser, driverForCreateDto.Password);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Gagal Menambahkan User");
                    }

                    var userResult = await _userManager.FindByNameAsync(newUser.Email);
                    await _userManager.AddToRoleAsync(userResult, "Driver");

                    var userEntity = new Driver
                    {
                        Username = driverForCreateDto.Username,
                        FirstName = driverForCreateDto.FirstName,
                        LastName = driverForCreateDto.LastName,
                        PhoneNumber = driverForCreateDto.PhoneNumber,
                        Email = driverForCreateDto.Email,
                        DriverLatitude = 0,
                        DriverLongitude = 0,
                        Balance = 0,
                        CreatedDate = DateTime.Now,
                        Blocked = true
                    };

                    Console.WriteLine(userEntity);

                    CreateDriver(userEntity);
                    SaveChanges();
                }
                else
                {
                    await AddRole("Driver");
                    throw new Exception($"Role Driver Belum Ada --> Menambhakan Role Driver");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<List<string>> GetRolesFromUser(string username)
        {
            try
            {
                List<string> lstRoles = new List<string>();
                var user = await _userManager.FindByEmailAsync(username);
                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    lstRoles.Add(role);
                }
                return lstRoles;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public void CreateDriver(Driver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }
            _context.Drivers.Add(driver);
        }

        public async Task AddRole(string rolename)
        {
            try
            {
                var roleIsExist = await _roleManager.RoleExistsAsync(rolename);
                if (roleIsExist)
                {
                    throw new Exception($"Role {rolename} sudah terdaftar");
                }

                else
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Profile
        public Driver ShowProfile()
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            return _context.Drivers.FirstOrDefault(dri => dri.Username == userName);
        }

        public void SetPosition(Driver obj)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (userName == null)
            {
                throw new Exception("Silahkan Login Terlebih Dahulu");
            }
            var result = _context.Drivers.FirstOrDefault(dri => dri.Username == userName);

            result.DriverLongitude = obj.DriverLongitude;
            result.DriverLatitude = obj.DriverLatitude;
            _context.SaveChanges();
        }

        //Order
        public void AcceptOrder(CustIdDto custIdDto)
        {
            var result = _driverDataClient.AcceptOrderToOrderService(custIdDto);

            if (result == null)
            {
                throw new Exception($"Order dengan Customer Id: {custIdDto.CustomerId} tidak di temukan");
            }
        }

        public void FinishOrder(CustIdDto custIdDto)
        {
            var result = _driverDataClient.FinishOrderToOrderService(custIdDto);

            if (result == null)
            {
                throw new Exception($"Order id {custIdDto.CustomerId} tidak di temukan");
            }
        }
    }
}
