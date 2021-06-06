using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Services.Orders.Dtos;

namespace Services.Orders.HttpServices
{
    public class CustomerHttpService : ICustomerHttpService
    {
        private HttpClient _client { get; }

        public CustomerHttpService(HttpClient client)
        {
            _client = client;
        }

        public async Task<BasketDto> GetBasket(Guid customerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"baskets/{customerId}");
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                response.Content.Dispose();
                return JsonConvert.DeserializeObject<BasketDto>(content);
            }
            throw new Exception("Customer service connection error");
        }
    }
}