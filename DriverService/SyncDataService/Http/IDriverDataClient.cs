using DriverService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.SyncDataService.Http
{
    public interface IDriverDataClient
    {
        Task SendDriverToOrderService(DriverForSendHttpDto driverForSendHttp);
        Task SetPositionToOrderServicee(SetPositionDto setPositionDto);
        Task <IEnumerable<OrderDto>> GetOrderFromOrderService();
        Task <IEnumerable<OrderDto>> GetHistoryOrderFromOrderService();
        Task AcceptOrderToOrderService(int custId);
        Task FinishOrderToOrderService(int custId);
    }
}
