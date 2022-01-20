using DriverService.Dtos;
using DriverService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.Data
{
    public interface IDriverRepo
    {
        bool SaveChanges();

        //Administration
        Task<User> Login(string username, string password);
        Task Registration(DriverForCreateDto driverForCreateDto);
        Task<List<string>> GetRolesFromUser(string username);

        //Profile
        Driver ShowProfile();
        Driver ShowSaldo();
        void SetPosition(Driver obj);

        //Order
        IEnumerable<OrderDto> GetAllOrders();
        IEnumerable<OrderDto> GetHistoryOrder();
        void AcceptOrder(int custId);
        void FinishOrder(int custId);
    }
}
