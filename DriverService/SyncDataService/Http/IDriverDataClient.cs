using DriverService.Dtos;
using DriverService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DriverService.SyncDataService.Http
{
    public interface IDriverDataClient
    {
        Task SendDriverToOrderService(DriverForSendHttpDto driverForSendHttp);
        Task SetPositionToOrderServicee(Driver driver);
        Task<IEnumerable<OrderDto>> GetOrderFromOrderService();
        Task <IEnumerable<OrderDto>> GetHistoryOrderFromOrderService();
        Task <ReadSaldoDto> GetSaldoDriver();
        Task AcceptOrderToOrderService(CustIdDto custIdDto);
        Task FinishOrderToOrderService(CustIdDto custIdDto);
    }
}
