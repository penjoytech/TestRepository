using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using StackExchange.Redis;

namespace CommonApplicationFramework.Caching
{
    public class CacheManager 
    {
        public static CacheManager instance = null;
        public static Lazy<ConnectionMultiplexer> _lazyConnection;

        public static ConnectionMultiplexer _connection;
        public static IDatabase _cacheStore = null;
   

        public CacheManager() { }

        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CacheManager();
                    var configurationOptions = new ConfigurationOptions
                    {
                        EndPoints = { "localhost" },
                        AbortOnConnectFail = false
                    };

                    _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
                    _connection = _lazyConnection.Value;
                    _cacheStore = _connection.GetDatabase();
                }
 
               
                return instance;
            }


        }

        public bool Exists(string key)
        {
            return _cacheStore.KeyExists(key);
        }

        public void Set(string key, string value)
        {
            var ts = TimeSpan.FromMinutes(30);
            _cacheStore.StringSet(key, value, ts);
        }

        public void Set(string key, string value, TimeSpan ts)
        {
           _cacheStore.StringSet(key, value, ts);
        }

        public string Get(string key)
        {
            return _cacheStore.StringGet(key);
        }

        public void Remove(string key)
        {
            _cacheStore.KeyDelete(key);
        }

        public void Clear()
        {
            var endpoints = _connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _connection.GetServer(endpoint);
                server.FlushDatabase();
            }
        }
        public void Update(string key, string value)
        {
            _cacheStore.StringSet(key, value);
        }

        public void Update(string key, string value, TimeSpan ts)
        {
            _cacheStore.StringSet(key, value, ts);
        }
    }
}
