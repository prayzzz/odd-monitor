using Newtonsoft.Json.Linq;

namespace OddMonitor.Common
{
    public static class Extension
    {
        public static string ValueAsString(this JToken token, string property)
        {
            return token.Value<string>(property).Trim();
        }
    }
}