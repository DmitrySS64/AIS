using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vkontakte
{
    public class VkRequestBuilder
    {
        private const string ApiBaseUrl = "https://api.vk.com/method/";
        private readonly string _accessToken;
        private readonly string _apiVersion;

        public VkRequestBuilder(string accessToken, string apiVersion = "5.199")
        {
            _accessToken = accessToken;
            _apiVersion = apiVersion;
        }

        // Метод для создания GET-запроса
        public async Task<string> SendGetRequestAsync(string methodName, Dictionary<string, string> parameters)
        {
            var url = BuildUrl(methodName, parameters);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        // Метод для создания POST-запроса
        public async Task<string> SendPostRequestAsync(string methodName, Dictionary<string, string> parameters)
        {
            var url = BuildUrl(methodName, null);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
                var content = new FormUrlEncodedContent(parameters);
                var response = await client.PostAsync(url, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        // Метод для формирования URL с query-параметрами
        private string BuildUrl(string methodName, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder($"{ApiBaseUrl}{methodName}");
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["access_token"] = _accessToken;
            query["v"] = _apiVersion;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    query[param.Key] = param.Value;
                }
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}
