using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Services.Customers.HttpServices
{
    public class BookHttpService : IBookHttpService
    {
        private HttpClient _client { get; }

        public BookHttpService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Data.Book> GetAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"books/{id}");
            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                response.Content.Dispose();
                return JsonConvert.DeserializeObject<Data.Book>(content);
            }
            throw new Exception("Book service connection error");
        }
    }
}