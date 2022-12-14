using HHServer.Core.HFMongoDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Core.Logger
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LoggerEntity : HFMongoEntityBase
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public LoggerLevel Level;

        /// <summary>
        /// 日志分类
        /// </summary>
        public int Category;

        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message;
    }
}
