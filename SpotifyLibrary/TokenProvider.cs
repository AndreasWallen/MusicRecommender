using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;

namespace SpotifyLibrary
{
    public class TokenProvider : ITokenProvider
    {
        private readonly string _spotifyClient = ConfigurationManager.AppSettings.Get("SpotifyClient");
        private readonly string _spotifySecret = ConfigurationManager.AppSettings.Get("SpotifySecret");
        private string _token;

        public string GetToken()
        {
            return _token ?? (_token = GetClientCredentialsAuthToken(_spotifyClient, _spotifySecret));
        }

        private static string GetClientCredentialsAuthToken(string spotifyClient, string spotifySecret)
        {
            var webClient = new WebClient();

            var postparams = new NameValueCollection { { "grant_type", "client_credentials" } };

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{spotifyClient}:{spotifySecret}"));
            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);

            var tokenRes = Encoding.UTF8.GetString(tokenResponse);

            var pFrom = tokenRes.IndexOf("access_token") + "access_token".Length;
            var pTo = tokenRes.LastIndexOf("token_type");
            var semiToken = tokenRes.Substring(pFrom, pTo - pFrom);
            var myAccessToken = semiToken.Substring(3, semiToken.Length - 6);

            return myAccessToken;
        }
    }
}
