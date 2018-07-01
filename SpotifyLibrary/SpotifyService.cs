using SpotifyLibrary.Extensions;
using SpotifyLibrary.Model.Source.Search;
using SpotifyLibrary.Model.Target;
using SpotifyLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace SpotifyLibrary
{
    public class SpotifyService
    {
        private const string SpotifyBaseUrl = "https://api.spotify.com/v1/";
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly ITokenProvider _tokenProvider;

        public SpotifyService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public IEnumerable<Artist> SearchArtists(string searchTerm)
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
                client.Headers = new WebHeaderCollection { { "Authorization", $"Bearer {_tokenProvider.GetToken()}" } };
                var jsonString = client.DownloadString($"{SpotifyBaseUrl}{address}");
                return jsonString.ToObject<TRootObject>();
            }
        }
    }








}
