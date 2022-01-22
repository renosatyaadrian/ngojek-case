using OrderService.Dtos;
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
        Driver ShowSaldo();

        //Order
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetHistoryOrder();
        void AcceptOrder(CustIdDto custIdDto);
        void FinishOrder(CustIdDto custIdDto);
    }
}
