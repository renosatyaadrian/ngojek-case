using DriverService.Dtos;
using DriverService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DriverService.SyncDataService.Http
{
    public class HttpOrderDataClient : IDriverDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpOrderDataClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            var accessToken = httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            httpClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", accessToken);
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task AcceptOrderToOrderService(CustIdDto custIdDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(custIdDto),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["AcceptOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync PutAsync Accepting Order to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync PutAsync Accepting Order to OrderService Failed");
            }
        }

        public async Task FinishOrderToOrderService(CustIdDto custIdDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(custIdDto),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["FinishOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync PutAsync Finishing Order to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync PutAsync Finishing Order to OrderService Failed");
            }
        }

        public async Task<IEnumerable<OrderDto>> GetHistoryOrderFromOrderService()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_configuration["OrderServiceHistory"]).Result;
            var results = await response.Content.ReadAsStringAsync();

            JArray resultarray = JArray.Parse(results);
            var result = resultarray.ToObject<IEnumerable<OrderDto>>();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Get History Order to Driver Service Was OK !");

                return result;
            }
            else
            {
                Console.WriteLine("--> Sync Get History Order to Driver Service Failed");

                throw new ArgumentNullException("Tidak terdapat order yang pernah anda ambil");
            }
        }

        public async Task<IEnumerable<OrderDto>> GetOrderFromOrderService()
        {

            HttpResponseMessage response = _httpClient.GetAsync(_configuration["GetOrder"]).Result;

            var results = await response.Content.ReadAsStringAsync();

            JArray resultarray = JArray.Parse(results);
            var result = resultarray.ToObject<IEnumerable<OrderDto>>();


            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Get Order to Driver Service Was OK !");

                return result;
            }
            else
            {
                Console.WriteLine("--> Sync Get Order to Driver Service Failed");

                throw new ArgumentNullException("Tidak terdapat order di sekitar anda");
            }
        }

        public async Task SetPositionToOrderServicee(Driver driver)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(driver),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_configuration["SetPositionOrderService"],
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync PUTAsync Set Position to OrderService Was OK !");
            }
            else
            {
                Console.WriteLine("--> Sync PUTAsync Set Position to OrderService Failed");
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

        public async Task<ReadSaldoDto> GetSaldoDriver()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_configuration["ShowSaldoDriver"]).Result;
            var results = await response.Content.ReadAsStringAsync();

            var resultbalance = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSaldoDto>(results);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Get Saldo Driver  Was OK !");
                return resultbalance;
            }
            else
            {
                Console.WriteLine("--> Sync Saldo Driver Failed");

                throw new ArgumentNullException("Tidak terdapat order yang pernah anda ambil");
            }
        }
    }
}
