﻿using System;

namespace HHServer.Core.Utils
{
    public class HFDateTimeUtil
    {
        #region GetTimestamp 获取时间戳

        /// <summary>
        /// 获取时间戳 定义为格林乔治时间1970年01月01日00时00分00秒起至现在的总秒速
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            return (DateTime.UtcNow.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static long GetTimestamp(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        #endregion
    }
}
