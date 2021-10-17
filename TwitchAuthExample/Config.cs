using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchAuthExample
{
    public static class Config
    {
        public static readonly string TwitchClientId = "<TWITCH_CLIENT_ID>";
        public static readonly string TwitchRedirectUri = "http://localhost:8080/redirect/";
        public static readonly string TwitchClientSecret = "<TWITCH_CLIENT_SECRET>";
    }
}
