using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vkontakte.Methods;

namespace Vkontakte
{
    public class VkApi
    {
        private readonly VkRequestBuilder _vkRequestBuilder;

        public VkApi(string accessToken)
        {
            _vkRequestBuilder = new VkRequestBuilder(accessToken);
        }

        // Метод для вызова account.getProfileInfo
        public async Task<string> GetProfileInfoAsync()
        {
            // Параметры для GetProfileInfo не требуются
            string response = await _vkRequestBuilder.SendGetRequestAsync(Methods.Account.GetProfileInfo, new Dictionary<string, string>());
            return response;
        }

        public async Task<Methods.Friends.FriendsInfo> GetFriendsAsync()
        {
            var parameters = new Dictionary<string, string>
            {
                //{ "user_id", "ID пользователя" } // Здесь можно указать конкретный user_id
            };

            var responseJson = await _vkRequestBuilder.SendGetRequestAsync(Methods.Friends.Get, parameters);

            var getResponse = JsonConvert.DeserializeObject<Methods.Friends.GetResponse>(responseJson);
            return getResponse?._profileInfo;
        }
        public async Task<IEnumerable<Methods.User.UserInfo>> GetUsersAsync(int[] ids)
        {
            var parameters = new Dictionary<string, string>
            {
                { "user_ids", string.Join(",", ids) }
                //{ "user_id", "ID пользователя" } // Здесь можно указать конкретный user_id
            };

            var responseJson = await _vkRequestBuilder.SendGetRequestAsync(Methods.User.Get, parameters);

            var getResponse = JsonConvert.DeserializeObject<Methods.User.GetResponse>(responseJson);
            return getResponse?.Users;
        }
    }
}
