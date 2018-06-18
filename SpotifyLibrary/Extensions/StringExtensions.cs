using Newtonsoft.Json;

namespace SpotifyLibrary.Extensions
{
    public static class StringExtensions
    {
        public static T ToObject<T>(this string str) => JsonConvert.DeserializeObject<T>(str);

    }
}
