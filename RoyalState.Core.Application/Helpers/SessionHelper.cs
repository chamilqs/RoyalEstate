using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RoyalState.Core.Application.Helpers
{
    public static class SessionHelper
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
#pragma warning disable CS8603 // Possible null reference return.
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
