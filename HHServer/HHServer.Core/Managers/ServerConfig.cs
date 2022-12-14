using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HHServer.Common.Managers
{
    /// <summary>
    /// 游戏服配置
    /// </summary>
    public sealed class ServerConfig
    {
        /// <summary>
        /// 服务器编号
        /// </summary>
        public static int ServerId = 1;

        /// <summary>
        /// Mongo链接字符串
        /// </summary>
        public static string MongoConnectionString = "mongodb://admin:123456@114.67.240.38:27017/admin";

        /// <summary>
        /// Redis链接字符串
        /// </summary>
        public static string RedisConnectionString = "114.67.240.38:6379,password=qq219673605";

        #region ServerDBName 服务器DBName
        private static string m_ServerDBName = null;

        /// <summary>
        /// 服务器DBName
        /// </summary>
        public static string ServerDBName
        {
            get
            {
                if (string.IsNullOrEmpty(m_ServerDBName))
                {
                    m_ServerDBName = $"Server_{ServerId}";
                }
                return m_ServerDBName;
            }
        }

        #endregion

        #region RoleHashKey 角色哈希Key
        private static string m_RoleHashKey = null;

        /// <summary>
        /// 角色哈希Key
        /// </summary>
        public static string RoleHashKey
        {
            get
            {
                if (string.IsNullOrEmpty(m_RoleHashKey))
                {
                    m_RoleHashKey = $"{ServerId}_RoleHash";
                }
                return m_RoleHashKey;
            }
        }


        #endregion
    }
}
