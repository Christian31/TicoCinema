using System.Web;

namespace TicoCinema.WebApplication.Utils
{
    public static class CacheManager
    {
        public static void AddValuesToCache<T>(this T httpContext, string key, object value) where T: HttpContextBase
        {
            if (!httpContext.KeyExistsOnCache(key))
            {
                httpContext.Cache.Insert(key, value);
            }
        }

        public static object GetValuesFromCache<T>(this T httpContext, string key) where T : HttpContextBase
        {
            return httpContext.Cache.Get(key);
        }

        public static bool KeyExistsOnCache<T>(this T httpContext, string key) where T : HttpContextBase
        {
            return HttpRuntime.Cache.Get(key) != null;
        }

        public static void RemoveValuesFromCache<T>(this T httpContext, string key) where T : HttpContextBase
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}