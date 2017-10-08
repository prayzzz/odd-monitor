using Newtonsoft.Json.Linq;

namespace OddMonitor.Common
{
    public static class JsonExtensions
    {
        public static string ValueAsString(this JToken token, string property)
        {
            return token.Value<string>(property).Trim();
        }
    }
}
