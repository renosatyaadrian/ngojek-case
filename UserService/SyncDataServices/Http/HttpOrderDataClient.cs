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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserService.Dtos;
using UserService.Helper;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace UserService.SyncDataServices.Http
{
    public class HttpOrderDataClient : IOrderDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<HttpClientSettings> _httpClientSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpOrderDataClient()
        {
        }

        public HttpOrderDataClient(HttpClient httpClient, IOptions<HttpClientSettings> httpClientSettings, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClientSettings = httpClientSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderDto> GetUserOrderById(int id, int orderId)
        {
            string tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = tokenHeader.Substring("Bearer ".Length).Trim();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string reqUri = _httpClientSettings.Value.UserController.ToString() + $"/{id}/Order/{orderId}";

            HttpResponseMessage response = await _httpClient.GetAsync(reqUri);
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
            
            string reqUri = _httpClientSettings.Value.UserController.ToString() + $"/{id}/Orders";

            HttpResponseMessage response = await _httpClient.GetAsync(reqUri);
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