using SpotifyLibrary.Extensions;
using SpotifyLibrary.Model.Source.Search;
using SpotifyLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using SpotifyLibrary.Model.Target;


namespace SpotifyLibrary
{
    public class SpotifyService
    {
        private const string SpotifyBaseUrl = "https://api.spotify.com/v1/";
        private const string SpotifyClient = "996d0037680544c987287a9b0470fdbb";
        private const string SpotifySecret = "5a3c92099a324b8f9e45d77e919fec13";
        private readonly Encoding _encoding;
        private readonly string _token;

        public SpotifyService(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
            _token = GetClientCredentialsAuthToken(SpotifyClient, SpotifySecret);
        }

        public IEnumerable<Artist> SearchArtists(string searchTerm)
        {
            try
            {
                var address = $"search?q={searchTerm}&type=artist";
                var rootObject = GetRootObjectByRequest<Model.Source.Search.RootObject>(address);
                var sourceArtists = rootObject.artists;
                if (sourceArtists.items.Count == 1)
                {
                    return GetRelatedArtist(sourceArtists.items.Single());
                }
                var itemsSortedByBestMatch = sourceArtists.items.OrderBy(x =>
                    Algorithms.ComputeLevenshteinDistance(x.name.ToUpper(), searchTerm.ToUpper()));
                return GetRelatedArtist(itemsSortedByBestMatch.First());
            }
            catch
            {
                return Enumerable.Empty<Artist>();
            }
        }

        private IEnumerable<Artist> GetRelatedArtist(Item item)
        {
            var address = $"artists/{item.id}/related-artists";
            var rootObject = GetRootObjectByRequest<Model.Source.Related.RootObject>(address);
            var sourceArtists = rootObject.artists;
            const int preferedImageHeight = 180;
            return sourceArtists.Select(source => new Artist
            {
                Name = source.name,
                RelatedArtistName = item.name,
                //Set image url to the image with the height closest to the prefered image height
                ImageUrl = source.images.OrderBy(image => Math.Abs(preferedImageHeight - image.height)).FirstOrDefault()?.url,
                Popularity = source.popularity
            }).OrderByDescending(artist => artist.Popularity).Take(10);
        }

        private TRootObject GetRootObjectByRequest<TRootObject>(string address)
        {
            using (var client = new WebClient { Encoding = _encoding })
            {
                client.Headers = new WebHeaderCollection { { "Authorization", $"Bearer {_token}" } };
                var jsonString = client.DownloadString($"{SpotifyBaseUrl}{address}");
                return jsonString.ToObject<TRootObject>();
            }
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
