using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace SpectacledBear.CodeMash2016.WebApi.ContractTests
{
    public class ConsumerHobbitApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort
        {
            get { return 1234; }
        }

        public string MockProviderServiceBaseUri
        {
            get { return string.Format("http://localhost:{0}", MockServerPort); }
        }

        public ConsumerHobbitApiPact()
        {
            PactBuilder = new PactBuilder();

            PactBuilder.ServiceConsumer("Consumer").HasPactWith("Hobbit API");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
