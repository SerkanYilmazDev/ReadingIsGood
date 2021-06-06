using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Services.Orders.Dtos;
using Xunit;

namespace ContractTests.Customers
{
    public class CustomersContractTests : IClassFixture<CustomersApiMock>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _serviceUri;

        public CustomersContractTests(CustomersApiMock fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _serviceUri = fixture.CustomerServiceUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public async Task should_return_customer_basket()
        {
            var id = Guid.Parse("5da9ff9b-a93a-4162-985f-39fddce5ea04");
            var customerId = Guid.Parse("0a5f9589-e0d7-4c2a-a9d2-941a2f5e650b");
            var basketId = Guid.Parse("6a9da4c4-2669-46d4-8bba-60557b6c81cc");
            var productId = Guid.Parse("0868e119-a2c2-4f35-a2a8-1e77362727d8");

            _mockProviderService
                .Given($"There is basket for customer: {customerId}")
                .UponReceiving("A GET request to retrieve customer basket.")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/api/baskets/{customerId}"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        CustomerId = Match.Type(customerId),
                        Items = new[]
                        {
                            new
                            {
                                Id = Match.Type(id),
                                BasketId = Match.Type(basketId),
                                ProductId = Match.Type(productId),
                                Quantity = Match.Type(5),
                                UnitPrice = Match.Type(1),
                                ProductName = Match.Type("Lorem Ipsum")
                            }
                        }
                    }
                });


            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_serviceUri}/api/baskets/{customerId}");
            var json = await response.Content.ReadAsStringAsync();
            var basket = JsonConvert.DeserializeObject<BasketDto>(json);

            Assert.Equal(basket.CustomerId, customerId);

            _mockProviderService.VerifyInteractions();
        }
    }
}