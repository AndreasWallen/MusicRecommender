using System.Collections.Generic;

namespace SpotifyLibrary.Model.Source.Search
{
    //Json to C# Models for search

    internal class ExternalUrls
    {
        public string spotify { get; set; }
    }

    internal class Followers
    {
        public object href { get; set; }
        public int total { get; set; }
    }

    internal class Item
    {
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public List<object> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<object> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    internal class Artists
    {
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }

    internal class RootObject
    {
        public Artists artists { get; set; }
    }
}
