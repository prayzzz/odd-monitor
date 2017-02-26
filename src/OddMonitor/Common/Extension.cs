using Newtonsoft.Json.Linq;

namespace PlayTheOdds.Common
{
    public static class Extension
    {
        public static string ValueAsString(this JToken token, string property)
        {
            return token.Value<string>(property).Trim();
        }
    }
}