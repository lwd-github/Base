using Common.Extension;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cache.Redis
{
    public class RedisHash
    {
        IDatabase _database;

        public RedisHash(IDatabase database)
        {
            _database = database;
        }


        public void Set(string key, string hashField, string value)
        {
            Set<string>(key, hashField, value);
        }


        public void Set<T>(string key, string hashField, T value)
        {
            var result = Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var valueFormat = type != typeof(string) ? value.ToJson() : value.ToString();
                return _database.HashSet(key, hashField, valueFormat);
            });

            if (!result)
            {
                throw new RedisException($"设置Hash键值失败，键名：{key}，字段名：{hashField}");
            }
        }


        public void Set<T>(string key, IDictionary<string, T> values)
        {
            Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var isString = type != typeof(string);
                //var valueFormat = type != typeof(string) ? value.ToJson() : value.ToString();
                var hashEntries = values.Select(t => new HashEntry(t.Key, isString ? t.Value.ToString() : t.Value.ToJson())).ToArray();
                _database.HashSet(key, hashEntries);
                return true;
            });
        }


        private T Do<T>(Func<IDatabase, T> func)
        {
            return func(_database);
        }
    }
}
