using OrderService.Models;
using System.Collections.Generic;

namespace OrderService.Data
{
    public interface IDriverRepo
    {
        bool SaveChanges();

        //Profile
        void CreateDriver(Driver driver);
        void SetPosition(Driver obj);

        //Order
        IEnumerable<Order> GetAllOrders();
        Order GetHistoryOrder();
        void AcceptOrder(int custId);
        void FinishOrder(int custId);
    }
}
