using DriverService.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DriverService.SyncDataService.Http
{
    public class HttpOrderDataClient : IDriverDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpOrderDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task AcceptOrderToOrderService(int custId)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(custId),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["AcceptOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");
            }
        }

        public async Task FinishOrderToOrderService(int custId)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(custId),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["FinishOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");
            }
        }

        public async Task<IEnumerable<OrderDto>> GetHistoryOrderFromOrderService()
        {
            var response = await _httpClient.GetAsync(_configuration["OrderServiceHistory"]);

            var httpContent = response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");

                return (IEnumerable<OrderDto>)httpContent;
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");

                throw new Exception("Tidak terdapat order di sekitar anda");
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrderFromOrderService()
        {

            var response = await _httpClient.GetAsync(_configuration["GetOrder"]);

            var httpContent = response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");

                return (IEnumerable<OrderDto>)httpContent;
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");

                throw new Exception("Tidak terdapat order di sekitar anda");
            }
        }

        public async Task SetPositionToOrderServicee(SetPositionDto setPositionDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(setPositionDto),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["SetPositionOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");
            }
        }

        public async Task SendDriverToOrderService(DriverForSendHttpDto driverForSendHttp)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(driverForSendHttp),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_configuration["PostDriver"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync POST to OrderService Failed");
            }
        }
    }
}
