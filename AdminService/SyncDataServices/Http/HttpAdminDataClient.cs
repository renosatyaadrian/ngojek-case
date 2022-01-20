using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AdminService.SyncDataServices.Http
{
    public class HttpAdminDataClient : IAdminDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpAdminDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomer()
        {
            var url = _configuration["CustomerService"];
            var response = await _httpClient.GetAsync($"{url}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to Customer Service was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync GET to Customer Service failed");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());

            }
            var value = JsonSerializer.Deserialize<IEnumerable<CustomerDto>>(await response.Content.ReadAsByteArrayAsync());
            return value;
        }

        public async Task<IEnumerable<DriverDto>> GetDriver()
        {
            var url = _configuration["DriverService"];
            var response = await _httpClient.GetAsync($"{url}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to Driver Service was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync GET to Driver Service failed");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            var value = JsonSerializer.Deserialize<IEnumerable<DriverDto>>(await response.Content.ReadAsByteArrayAsync());
            return value;
        }

        public async Task<IEnumerable<OrderDto>> GetOrder()
        {
            var url = _configuration["OrderService"];
            var response = await _httpClient.GetAsync($"{url}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync GET to Order Service was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync GET to Order Service failed");
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            var value = JsonSerializer.Deserialize<IEnumerable<OrderDto>>(await response.Content.ReadAsByteArrayAsync());
            return value;
        }
    }
}
