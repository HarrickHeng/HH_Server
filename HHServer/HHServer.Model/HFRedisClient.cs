using CSRedis;
using HHServer.Common.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Model
{
    /// <summary>
    /// RedisClient
    /// </summary>
    public static class HFRedisClient
    {
        private static object lock_obj = new object();

        private static CSRedisClient m_CurrClient = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitRedisClient()
        {
            if (m_CurrClient == null)
            {
                lock (lock_obj)
                {
                    if (m_CurrClient == null)
                    {
                        m_CurrClient = new CSRedisClient(ServerConfig.RedisConnectionString);
                        RedisHelper.Initialization(m_CurrClient);
                    }
                }
            }
        }
    }
}
