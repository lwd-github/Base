using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Cache.Redis
{
    public class RedisSortedSet
    {
        Func<IDatabase> _getDatabase;

        public RedisSortedSet(Func<IDatabase> getDatabase)
        {
            _getDatabase = getDatabase;
        }


        /// <summary>
        /// 如果指定的成员已经是排序集的成员，则会更新分数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd(string key, string member, double score)
        {
            return Do(db =>
            {
                return db.SortedSetAdd(key, member, score);
            });
        }


        public bool SortedSetRemove(string key, string member)
        {
            return Do(db =>
            {
                return db.SortedSetRemove(key, member);
            });
        }


        public double SortedSetIncrement(string key, string member, double value)
        {
            return Do(db =>
            {
                return db.SortedSetIncrement(key, member, value);
            });
        }


        public IEnumerable<string> SortedSetRangeByRank(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return Do(db =>
            {
                var values = db.SortedSetRangeByRank(key, start, stop, (Order)order);
                return values.Select(t => (string)Convert.ChangeType(t, typeof(string)));
            });
        }


        /// <summary>
        /// 获取成员的排名
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public long? SortedSetRank(string key, string member, RedisOrder order = RedisOrder.Ascending)
        {
            return Do(db =>
            {
                return db.SortedSetRank(key, member, (Order)order);
            });
        }


        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _getDatabase();
            return func(database);
        }
    }
}
