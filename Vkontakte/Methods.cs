using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Vkontakte.Methods.Account;

namespace Vkontakte
{
    public static class Methods
    {
        public const string https = "https://api.vk.com/method/";
        public readonly struct Account
        {
            private const string Prefix = "account";
            public static string GetProfileInfo => $"{Prefix}.getProfileInfo";
            public class GetProfileInfoResponse
            {
                [JsonProperty("response")]
                public ProfileInfo _profileInfo { get; set; }
            }
            public class ProfileInfo
            {
                [JsonProperty("id")]
                public int Id { get; set; }
                [JsonProperty("first_name")]
                public string FirstName { get; set; }
                [JsonProperty("last_name")]
                public string LastName { get; set; }
                [JsonProperty("city")]
                public City city { get; set; }
                [JsonProperty("status")]
                public string Status { get; set; }
                [JsonProperty("bdate")]
                public string BirthDate { get; set; }
                [JsonProperty("phone")]
                public string Phone { get; set; }
                [JsonProperty("screen_name")]
                public string ScreenName { get; set; }
                [JsonProperty("photo_200")]
                public string ProfilePhoto { get; set; }
                [JsonProperty("verification_status")]
                public string VerificationStatus { get; set; }
                [JsonProperty("sex")]
                public int Sex { get; set; }
                [JsonProperty("relation")]
                public int Relation { get; set; }
            }
            public class City
            {
                [JsonProperty("id")]
                public string Id { get; set; }
                [JsonProperty("title")]
                public string Title { get; set; }
            }
        }

        public readonly struct Groups
        {
            private const string Prefix = "groups";
            public static string Get => $"{Prefix}.get";
        }
        public readonly struct Friends
        {
            private const string Prefix = "friends";
            public static string Get => $"{Prefix}.get";

            public class GetResponse
            {
                [JsonProperty("response")]
                public FriendsInfo _profileInfo { get; set; }

            }
            public class FriendsInfo
            {
                [JsonProperty("count")]
                public int Count { get; set; }
                [JsonProperty("items")]
                public IEnumerable<int> Items { get; set; }
            }
        }
        public readonly struct User
        {
            private const string Prefix = "users";
            public static string Get => $"{Prefix}.get";

            public class GetResponse
            {
                [JsonProperty("response")]
                public IEnumerable<UserInfo> Users { get; set; }

            }
            public class UserInfo
            {
                [JsonProperty("id")]
                public int Id { get; set; }
                [JsonProperty("first_name")]
                public string FirstName { get; set; }
                [JsonProperty("last_name")]
                public string LastName { get; set; }
            }
        }
    }
}
