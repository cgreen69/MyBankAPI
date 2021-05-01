using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBank.API.Services
{
    public interface IAPIService {


            Task<T> GetAsync<T>(UriBuilder builder);

     }
    

    public class APIService : IAPIService 
    {
        private IHttpClientFactory clientFactory;

        
        public APIService(IHttpClientFactory clientFactory) {
            this.clientFactory = clientFactory;
        }

        public async Task<T> GetAsync<T>(UriBuilder builder)  {


            var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

            using var client = this.clientFactory.CreateClient();

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) { 
                throw new Exception("Unable to obtain value from remote api");
            }

                using var responseStream = await response.Content.ReadAsStreamAsync();

                var exRate = await JsonSerializer.DeserializeAsync<T>(responseStream);

                return exRate;
                


            
        }

    }

}

