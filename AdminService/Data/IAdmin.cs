using AdminService.Dtos;
using AdminService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.Data
{
    public interface IAdmin
    {
            bool SaveChanges();

            //Administration
            Task<User> Login(string username, string password);
            Task Registration(AdminCreateDto adminCreateDto);
            Task AddRoleForUser(string username, string role);
            Task<List<string>> GetRolesFromUser(string username);
            Task<User> Authenticate(string username, string password);

            //Driver
            Task<Driver> ApproveDriver(string driverId, Driver driver);
            Task<Driver> BlockDriver(string driverId, Driver driver);

            //User
            Task<User> BlockUsere(string userId, User user);

            //Transaction
            Task <IEnumerable<Order>> GetAllTransactions();
            Task<ConfigApp> SetPricePerKM(string id, ConfigApp configApp);
    }
}
