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


        public string Get(string key, string hashField)
        {
            return Get<string>(key, hashField);
        }


        public T Get<T>(string key, string hashField)
        {
            return Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var value = db.HashGet(key, hashField);

                if (value.IsNull)
                {
                    return default;
                }
                else if (type == typeof(string))
                {
                    return (T)Convert.ChangeType(value, type);
                }
                else
                {
                    return value.ToString().ToObject<T>();
                }
            });
        }


        public IList<string> Get(string key, string[] hashFields)
        {
            return Get<string>(key, hashFields);
        }


        public IList<T> Get<T>(string key, string[] hashFields)
        {
            return Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var fields = hashFields.Select(t => (RedisValue)t).ToArray();
                var values = db.HashGet(key, fields).Where(t => t.HasValue);

                if (type == typeof(string))
                {
                    return values.Select(t => (T)Convert.ChangeType(t, type)).ToList();
                }
                else
                {
                    return values.Select(t => t.ToString().ToObject<T>()).ToList();
                }
            });
        }


        public IDictionary<string, string> GetAll(string key)
        {
            return GetAll<string>(key);
        }


        public IDictionary<string, T> GetAll<T>(string key)
        {
            return Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var values = db.HashGetAll(key);

                if (type == typeof(string))
                {
                    return values.ToDictionary(t => t.Name.ToString(), t => (T)Convert.ChangeType(t.Value, type));
                }
                else
                {
                    return values.ToDictionary(t => t.Name.ToString(), t => t.Value.ToString().ToObject<T>());
                }
            });
        }


        public IList<string> GetValues(string key)
        {
            return GetValues<string>(key);
        }


        public IList<T> GetValues<T>(string key)
        {
            return Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var values = db.HashValues(key);

                if (type == typeof(string))
                {
                    return values.Select(t => t.IsNull ? default : (T)Convert.ChangeType(t, type)).ToList();
                }
                else
                {
                    return values.Select(t => t.IsNull ? default : t.ToString().ToObject<T>()).ToList();
                }
            });
        }


        public void Set(string key, string hashField, string value)
        {
            Set<string>(key, hashField, value);
        }


        public void Set<T>(string key, string hashField, T value)
        {
            Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var valueFormat = type != typeof(string) ? value.ToJson() : value.ToString();

                //如果字段是哈希中的新字段并且设置了值，则返回1。如果哈希中已存在字段且值已更新，则为0。
                return _database.HashSet(key, hashField, valueFormat);
            });
        }


        public void Set<T>(string key, IDictionary<string, T> values)
        {
            Do(db =>
            {
                Type type = typeof(T).GetUnderlyingType();
                var isString = type == typeof(string);
                //var valueFormat = type != typeof(string) ? value.ToJson() : value.ToString();
                var hashEntries = values.Select(t => new HashEntry(t.Key, isString ? t.Value.ToString() : t.Value.ToJson())).ToArray();
                _database.HashSet(key, hashEntries);
                return true;
            });
        }


        public bool Exists(string key, string hashField)
        {
            return Do(db => db.HashExists(key, hashField));
        }


        public void Remove(string key, string hashField)
        {
            Do(db => db.HashDelete(key, hashField));
        }


        public void Remove(string key, string[] hashFields)
        {
            Do(db => 
            {
                var fields = hashFields.Select(it => (RedisValue)it).ToArray();
                return db.HashDelete(key, fields);
            });
        }


        private T Do<T>(Func<IDatabase, T> func)
        {
            return func(_database);
        }
    }
}
