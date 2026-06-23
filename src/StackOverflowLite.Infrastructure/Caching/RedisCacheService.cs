using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Infrastructure.Caching;

public class RedisCacheService(
    IConnectionMultiplexer multiplexer,
    ILogger<RedisCacheService> logger) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private IDatabase Db => multiplexer.GetDatabase();

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var value = await Db.StringGetAsync(WithPrefix(key));
            if (value.IsNullOrEmpty) return null;
            return JsonSerializer.Deserialize<T>(value.ToString(), JsonOptions);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache GET failed for key {Key}. Treating as cache miss.", key);
            return null;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var payload = JsonSerializer.Serialize(value, JsonOptions);
            await Db.StringSetAsync(WithPrefix(key), payload, ttl);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache SET failed for key {Key}. Value not cached.", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await Db.KeyDeleteAsync(WithPrefix(key));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache REMOVE failed for key {Key}.", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        try
        {
            var pattern = WithPrefix(prefix) + "*";
            foreach (var endpoint in multiplexer.GetEndPoints())
            {
                var server = multiplexer.GetServer(endpoint);
                if (!server.IsConnected || server.IsReplica) continue;

                var batch = Db.CreateBatch();
                var pending = new List<Task>();
                await foreach (var key in server.KeysAsync(pattern: pattern, pageSize: 100).WithCancellation(cancellationToken))
                {
                    pending.Add(batch.KeyDeleteAsync(key));
                }
                batch.Execute();
                await Task.WhenAll(pending);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Cache REMOVE-BY-PREFIX failed for prefix {Prefix}.", prefix);
        }
    }

    private string WithPrefix(string key) => "StackOverflowLite:" + key;
}
