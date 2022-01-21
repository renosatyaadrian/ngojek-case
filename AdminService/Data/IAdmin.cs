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
            Task<IEnumerable<Driver>> GetAllDriver();
            void ApproveDriver(int driverId);
            void BlockDriver(int driverId);

            //User
            Task<IEnumerable<Customer>> GetAllCustomer();
            void BlockCustomer(int customerId);
            void UnblockCustomer(int customerId);

            //Transaction
            Task<IEnumerable<Order>> GetAllTransaction();
            Task<ConfigApp> GetPriceById(int Id);
            Task<ConfigApp> SetPricePerKM(ConfigApp configApp);
            Task<ConfigApp> UpdatePricePerKM(int Id, ConfigApp configApp);
    }
}
