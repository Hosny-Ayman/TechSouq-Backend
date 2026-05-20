using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;

namespace TechSouq.Application.Helper
{
    public static class ClearProductPagesCache
    {
        public static async Task<int> ClearProductPagesCacheAsync(IConnectionMultiplexer redis)
        {
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints.First());
            var keys = server.Keys(pattern: "TechSouq_Product_Page_*").ToArray();

            if (keys.Any())
            {
                var db = redis.GetDatabase();
                await db.KeyDeleteAsync(keys);
            }

            return keys.Length;
        }
    }
}