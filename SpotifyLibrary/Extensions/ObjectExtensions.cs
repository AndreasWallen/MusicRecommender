using Newtonsoft.Json;

namespace SpotifyLibrary.Extensions
{
    /// <summary>
    /// Extension methods for objects.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serializes the object to a json string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="indent">Indent child objects. As default they will be indented.</param>
        public static string ToJsonString<T>(this T obj, bool indent = true)
            where T : class => JsonConvert.SerializeObject(obj, indent ? Formatting.Indented : Formatting.None);
    }
}
