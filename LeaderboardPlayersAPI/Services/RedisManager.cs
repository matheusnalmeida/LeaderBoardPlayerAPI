using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LeaderboardPlayersAPI
{
    public class RedisManager
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisManager(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public int getNumberOfElements() {
            var playersList = this.SelectAll().Result;
            return playersList.Count;   
        }

        public async Task<bool> Insert(string key,string value) {
            var db = _connectionMultiplexer.GetDatabase();
            var dataRetrived = Select(key).Result;

            return await db.StringSetAsync(key, value);
        }

        public async Task<string> Select(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<List<string>> SelectAll()
        {
            EndPoint endPoint = _connectionMultiplexer.GetEndPoints().First();
            List<RedisKey> keys = _connectionMultiplexer.GetServer(endPoint).Keys(pattern: "*").ToList();
            
            List<string> allData = new List<string>();
            foreach (var key in keys)
            {
                var atualcKey = key.ToString();
                allData.Add(Select(atualcKey).Result);
            }

            return allData;
        }

        public async Task<bool> Update(string key, string value) {
            var dataRetrived = Select(key).Result;
            if (string.IsNullOrEmpty(dataRetrived)) {
                return false;
            }

            return await Insert(key, value);
        }

        public async Task<bool> Delete(string key) {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }
    }
}
