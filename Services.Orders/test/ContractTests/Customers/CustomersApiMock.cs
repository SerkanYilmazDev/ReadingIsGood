using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace ContractTests.Customers
{
    public class CustomersApiMock : IDisposable
    {
        private readonly IPactBuilder _pactBuilder;
        private readonly int _servicePort = 5005;
        public IMockProviderService MockProviderService { get; }
        public string CustomerServiceUri => $"http://localhost:{_servicePort}";

        public CustomersApiMock()
        {
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = @"..\..\..\..\..\..\_pacts",
                LogDir = @"pact_logs"
            };

            _pactBuilder = new PactBuilder(pactConfig)
                .ServiceConsumer("orders")
                .HasPactWith("customers");

            MockProviderService = _pactBuilder.MockService(_servicePort, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _pactBuilder.Build();
                }

                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}