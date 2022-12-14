using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HHServer.Core.Utils
{
    public class HFRedisHelper : RedisHelper
    {
        #region 哈希CacheSheell

        /// <summary>
        /// 哈希CacheSheell
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="getData"></param>
        /// <returns></returns>
        public static T HFCacheSheell<T>(string key, string field, Func<string, T> getData)
        {
            T t = HGet<T>(key, field);
            if (t == null)
            {
                t = getData(field);
                HSet(key, field, t);
            }
            return t;
        }

        #endregion

        #region 哈希HFCacheSheellAsync

        /// <summary>
        /// 哈希HFCacheSheellAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="getDataAsync"></param>
        /// <returns></returns>
        public static async Task<T> HFCacheSheellAsync
            <T>(string key, string field, Func<string, Task<T>> getDataAsync)
        {
            T t = await HGetAsync<T>(key, field);
            if (t == null)
            {
                t = await getDataAsync(field);
                await HSetAsync(key, field, t);
            }
            return t;
        }

        #endregion
    }
}
