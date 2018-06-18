using System.Collections.Generic;

namespace SpotifyLibrary.Model.Source.Related
{
    //Json to C# Models for related-artists
    internal class ExternalUrls
    {
        public string spotify { get; set; }
    }

    internal class Followers
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    internal class Image
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    internal class Artist
    {
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    internal class RootObject
    {
        public List<Artist> artists { get; set; }
    }
}
