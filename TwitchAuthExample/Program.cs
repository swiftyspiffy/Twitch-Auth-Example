using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Api.Core.Enums;

namespace TwitchAuthExample
{
    class Program
    {
        private static List<string> scopes = new List<string> { "chat:read", "whispers:read", "whispers:edit", "chat:edit", "channel:moderate" };

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("Twitch user access token example.");

            // ensure client id, secret, and redrect url are set
            validateCreds();

            // create twitch api instance
            var api = new TwitchLib.Api.TwitchAPI();
            api.Settings.ClientId = Config.TwitchClientId;

            // start local web server
            var server = new WebServer();
            server.OnAuthorization += async (s, e) =>
            {
                var resp = await api.Auth.GetAccessTokenFromCodeAsync(e.Code, Config.TwitchClientSecret, Config.TwitchRedirectUri);
                api.Settings.AccessToken = resp.AccessToken; // the users api call requires the access token be set
                var user = (await api.Helix.Users.GetUsersAsync()).Users[0];
                Console.WriteLine($"Authorization success!\n\nUser: {user.DisplayName} (id: {user.Id})\nAccess token: {resp.AccessToken}\nRefresh token: {resp.RefreshToken}\nExpires in: {resp.ExpiresIn}\nScopes: {string.Join(", ", resp.Scopes)}");
            };
            server.Start();

            // print out auth url
            Console.WriteLine($"Please authorize here:\n{getAuthorizationCodeUrl(Config.TwitchClientId, Config.TwitchRedirectUri, scopes)}");

            Console.ReadLine();
        }

        private static string getAuthorizationCodeUrl(string clientId, string redirectUri, List<string> scopes)
        {
            var scopesStr = String.Join('+', scopes);

            return "https://id.twitch.tv/oauth2/authorize?" +
                   $"client_id={clientId}&" +
                   $"redirect_uri={System.Web.HttpUtility.UrlEncode(redirectUri)}&" +
                   "response_type=code&" +
                   $"scope={scopesStr}";
        }

        private static void validateCreds()
        {
            if (String.IsNullOrEmpty(Config.TwitchClientId))
                throw new Exception("client id cannot be null or empty");
            if (String.IsNullOrEmpty(Config.TwitchClientSecret))
                throw new Exception("client secret cannot be null or empty");
            if (String.IsNullOrEmpty(Config.TwitchRedirectUri))
                throw new Exception("redirect uri cannot be null or empty");
            Console.WriteLine($"Using client id '{Config.TwitchClientId}', secret '{Config.TwitchClientSecret}' and redirect url '{Config.TwitchRedirectUri}'.");
        }
    }
}
