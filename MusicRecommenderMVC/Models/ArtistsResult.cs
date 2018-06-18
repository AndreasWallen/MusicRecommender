using System.Collections.Generic;
using System.Collections.ObjectModel;
using SpotifyLibrary.Model.Target;

namespace MusicRecommenderMVC.Models
{
    public class ArtistsResult
    {
        public string OriginalArtistName { get; }
        public ReadOnlyCollection<Artist> RelatedArtists { get; }

        public ArtistsResult(string originalArtistName, IList<Artist> relatedArtists)
        {
            OriginalArtistName = originalArtistName;
            RelatedArtists = new ReadOnlyCollection<Artist>(relatedArtists);
        }
    }
}