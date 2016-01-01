using System;
using RestSharp;
using Xunit;

namespace SpectacledBear.CodeMash2016.WebApi.BuildValidationTests
{
    [Trait("category", "ws")]
    public class WebApiValidationTests
    {
        private readonly Uri _webserviceRootUrl = new Uri("http://localhost:11341/api/");

        [Theory]
        [InlineData("hobbit/")]
        [InlineData("monitoring/")]
        public void Routes_ReturnData(string route)
        {
            IRestClient client = new RestClient(_webserviceRootUrl);
            IRestRequest request = new RestRequest(route, Method.GET);

            IRestResponse response = client.Execute(request);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.False(string.IsNullOrEmpty(response.Content));
        }
    }
}
