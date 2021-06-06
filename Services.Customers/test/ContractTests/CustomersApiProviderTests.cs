using System;
using System.Collections.Generic;
using PactNet;
using PactNet.Infrastructure.Outputters;
using ContractTests.Outputters;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace ContractTests
{
    public class CustomersApiProviderTests : IDisposable
    {
        private string _providerUri { get; }
        private IWebHost _webHost { get; }
        private ITestOutputHelper _output { get; }

        public CustomersApiProviderTests(ITestOutputHelper output)
        {
            _output = output;
            _providerUri = "http://localhost:5005";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_providerUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public void should_verify_pacts()
        {
            var pactConfig = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutputter(_output)
                },
                Verbose = true
            };

            new PactVerifier(pactConfig)
                .ServiceProvider("customers", _providerUri)
                .HonoursPactWith("orders")
                .PactUri(@"..\..\..\..\..\..\_pacts\orders-customers.json")
                .Verify();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}