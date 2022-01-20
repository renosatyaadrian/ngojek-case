using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserService.Dtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UserService.SyncDataServices.Http
{
    public class HttpOrderDataClient : IOrderDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpOrderDataClient()
        {
        }

        public HttpOrderDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<OrderDto> GetUserOrderById(int id, int orderId)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(String.Empty),
                Encoding.UTF8, "application/json"
            );
            HttpResponseMessage response = await _httpClient.PostAsync(
                $"http://localhost:7000/api/User/{id}/Order/{orderId}", httpContent);
            // var response = await _httpClient.PostAsync(_configuration.GetSection("OrderService").GetValue<string>("GetUserOrderById"), httpContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            OrderDto orderDto = JsonConvert.DeserializeObject<OrderDto>(responseBody);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to Command Service Success -->");
            }
            else
            {
                Console.WriteLine("--> Sync Post to Command Service Failed -->");
            }
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersHistory(int id)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(String.Empty),
                Encoding.UTF8, "application/json"
            );
            HttpResponseMessage response = await _httpClient.PostAsync(
                $"http://localhost:7000/api/User/{id}/Orders", httpContent);
            // var response = await _httpClient.PostAsync(_configuration.GetSection("OrderService").GetValue<string>("GetUserOrderById"), httpContent);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            IEnumerable<OrderDto> orderDtos = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(responseBody);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to Command Service Success -->");
            }
            else
            {
                Console.WriteLine("--> Sync Post to Command Service Failed -->");
            }
            return orderDtos;
        }
    }
}