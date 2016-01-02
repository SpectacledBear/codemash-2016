using System;
using System.Net.Http;
using PactNet;
using Xunit;

namespace SpectacledBear.CodeMash2016.WebApi.ContractTests
{
    [Trait("category", "service")]
    public class HobbitServiceTests
    {
        private readonly Uri _serviceUrl = new Uri("http://localhost:11341");

        [Fact]
        public void EnsureHobbitApiHonoursPactWithConsumer()
        {
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { });

            pactVerifier
                .ProviderState("There are hobbits");

            using (var client = new HttpClient { BaseAddress = _serviceUrl })
            {
                pactVerifier
                    .ServiceProvider("Hobbit API", client)
                    .HonoursPactWith("Consumer")
                    .PactUri("../../pacts/consumer-hobbit_api.json")
                    .Verify();
            }
        }
    }
}
