using System.Collections.Generic;
using System.Linq;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.ContractTests
{
    [Trait("category", "consumer")]
    public class HobbitConsumerTests : IClassFixture<ConsumerHobbitApiPact>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public HobbitConsumerTests(ConsumerHobbitApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            data.MockProviderService.ClearInteractions();
        }

        [Fact]
        public void GetHobbits_WhenTheHobbitsExist_ReturnsHobbits()
        {
                _mockProviderService
                    .Given("There are hobbits")
                    .UponReceiving("A GET request to retrieve the hobbits")
                    .With(new ProviderServiceRequest
                    {
                        Method = HttpVerb.Get,
                        Path = "/api/hobbit",
                        Headers = new Dictionary<string, string>
                        {
                            { "Accept", "application/json" }
                        }
                    })
                    .WillRespondWith(new ProviderServiceResponse
                    {
                        Status = 200,
                        Headers = new Dictionary<string, string>
                        {
                            { "Content-Type", "application/json; charset=utf-8" }
                        },
                        Body = new Hobbit[]
                        {
                              new Hobbit("Frodo Baggins", "Baggins", 1368, 1421, 1),
                              new Hobbit("Samwise Gamgee", "Gamgee", 1380, 1482, 2)
                        }
                    });

                var consumer = new HobbitApiClient(_mockProviderServiceBaseUri);

                var result = consumer.GetHobbits();
                Hobbit hobbit = result.FirstOrDefault();

                Assert.Equal("Frodo Baggins", hobbit.Name);
        }
    }
}
