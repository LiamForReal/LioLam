using Newtonsoft.Json;

namespace RestaurantWebApplication.externals
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            string json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string json = session.GetString(key);
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
