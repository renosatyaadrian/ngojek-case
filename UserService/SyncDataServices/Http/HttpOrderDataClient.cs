using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpOrderDataClient()
        {
        }

        public HttpOrderDataClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderDto> GetUserOrderById(int id, int orderId)
        {
            string tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = tokenHeader.Substring("Bearer ".Length).Trim();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(
                $"http://localhost:7000/api/User/{id}/Order/{orderId}");
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
            string tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = tokenHeader.Substring("Bearer ".Length).Trim();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(
                $"http://localhost:7000/api/User/{id}/Orders");
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