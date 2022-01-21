using AdminService.Dtos;
using AdminService.Helper;
using AdminService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdminService.Data
{
    public class AdminDAL : IAdmin
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppSettings _appSettings;
        public AdminDAL(ApplicationDbContext context,
                          IHttpContextAccessor httpContextAccessor,
                          UserManager<IdentityUser> userManager,
                          RoleManager<IdentityRole> roleManager,
                          IOptions<AppSettings> appSetings)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = appSetings.Value;
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0);
        }

        //Administration
        public async Task<User> Login(string username, string password)
        {
            try
            {
                var userFind = await _userManager.CheckPasswordAsync(await _userManager.FindByNameAsync(username), password);
                if (!userFind)
                {
                    return null;
                }

                var user = new User
                {
                    Username = username
                };

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
                var result = await _userManager.FindByEmailAsync(username);
                var roles = await _userManager.GetRolesAsync(result);

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
            catch (Exception ex)
            {

                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task Registration(AdminCreateDto adminCreateDto)
        {
            try
            {
                var newUser = new IdentityUser
                {
                    UserName = adminCreateDto.Username,
                    Email = adminCreateDto.Username

                };

                var result = await _userManager.CreateAsync(newUser, adminCreateDto.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Gagal Menambahkan User");
                }
                var userResult = await _userManager.FindByNameAsync(newUser.Email);

                var userEntity = new Admin
                {
                    Username = adminCreateDto.Username,
                    FullName = adminCreateDto.Fullname,
                    Email = adminCreateDto.Email
                };

                Console.WriteLine(userEntity);

                _dbContext.Admins.Add(userEntity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task AddRoleForUser(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            try
            {
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {

                    StringBuilder errMsg = new StringBuilder(String.Empty);
                    foreach (var err in result.Errors)
                    {
                        errMsg.Append(err.Description + " ");
                    }
                    throw new Exception($"{errMsg}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var userFind = await _userManager.CheckPasswordAsync(
                await _userManager.FindByNameAsync(username), password);
            if (!userFind)
                return null;
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


        //Driver
        public async Task<IEnumerable<Driver>> GetAllDriver()
        {
            var results = await(from d in _dbContext.Drivers
                                orderby d.FirstName ascending
                                select d).ToListAsync();
            return results;
        }

        public void ApproveDriver(int driverId)
        {
            var result = _dbContext.Drivers.FirstOrDefault(drv => drv.Id == driverId);

            if (result == null)
            {
                throw new Exception($"Driver id {result.Id} tidak di temukan");
            }

            result.Id = driverId;
            result.Blocked = false;
            _dbContext.SaveChanges();
        }

        public void BlockDriver(int driverId)
        {
            var result = _dbContext.Drivers.FirstOrDefault(drv => drv.Id == driverId);

            if (result == null)
            {
                throw new Exception($"Driver id {result.Id} tidak di temukan");
            }

            result.Id = driverId;
            result.Blocked = true;
            _dbContext.SaveChanges();
        }

        //User
        public async Task<IEnumerable<Customer>> GetAllCustomer()
        {
            var results = await (from c in _dbContext.Customers
                                 orderby c.FirstName ascending
                                 select c).ToListAsync();
            return results;
        }

        public void BlockCustomer(int customerId)
        {
            var result = _dbContext.Customers.FirstOrDefault(cust => cust.Id == customerId);

            if (result == null)
            {
                throw new Exception($"Customer id {result.Id} tidak di temukan");
            }

            result.Id = customerId;
            result.Blocked = true;
            _dbContext.SaveChanges();
        }

        public void UnblockCustomer(int customerId)
        {
            var result = _dbContext.Customers.FirstOrDefault(cust => cust.Id == customerId);

            if (result == null)
            {
                throw new Exception($"Customer id {result.Id} tidak di temukan");
            }

            result.Id = customerId;
            result.Blocked = false;
            _dbContext.SaveChanges();
        }

        //Transaction
        public async Task<IEnumerable<Order>> GetAllTransaction()
        {
            var results = await (from o in _dbContext.Orders
                                 orderby o.Id ascending
                                 select o).ToListAsync();
            return results;
        }

        public async Task<ConfigApp> GetPriceById(int Id)
        {
            var result = await _dbContext.ConfigApps.Where(s => s.Id == Id).SingleOrDefaultAsync();
            if (result != null)
                return result;
            else
                throw new Exception("Data tidak ditemukan !");
        }

        public async Task<ConfigApp> SetPricePerKM(ConfigApp configApp)
        {
            try
            {
                _dbContext.ConfigApps.Add(configApp);
                await _dbContext.SaveChangesAsync();
                return configApp;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }

        public async Task<ConfigApp> UpdatePricePerKM(int Id, ConfigApp configApp)
        {
            try
            {
                var result = await GetPriceById(Id);
                result.PricePerKM = configApp.PricePerKM;
                await _dbContext.SaveChangesAsync();
                configApp.Id = Id;
                return configApp;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Error: {dbEx.Message}");
            }
        }
    }
}
