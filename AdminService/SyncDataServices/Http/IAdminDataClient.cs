using AdminService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminService.SyncDataServices.Http
{
    public interface IAdminDataClient
    {
        Task<IEnumerable<CustomerDto>> GetCustomer();
        Task<IEnumerable<DriverDto>> GetDriver();
    }
}
