using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using SpectacledBear.CodeMash2016.WebApi.Models;

namespace SpectacledBear.CodeMash2016.WebApi.ContractTests
{
    public class HobbitApiClient
    {
        public string BaseUri { get; set; }

        public HobbitApiClient(string baseUri)
        {
            BaseUri = baseUri;
        }

        public IEnumerable<Hobbit> GetHobbits()
        {
            string reasonPhrase;

            using (var client = new HttpClient { BaseAddress = new Uri(BaseUri) })
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/hobbit");
                request.Headers.Add("Accept", "application/json");

                var response = client.SendAsync(request);

                var content = response.Result.Content.ReadAsStringAsync().Result;
                var status = response.Result.StatusCode;

                reasonPhrase = response.Result.ReasonPhrase;

                request.Dispose();
                response.Dispose();

                if (status == HttpStatusCode.OK)
                {
                    return !string.IsNullOrEmpty(content) ?
                        JsonConvert.DeserializeObject<IEnumerable<Hobbit>>(content)
                        : null;
                }

                throw new Exception(reasonPhrase);
            }
        }
    }
}
