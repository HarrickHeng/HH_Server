using HHServer.Core.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HHServer.Core.Logger
{
    public class LoggerMgr
    {
        private static readonly object lock_obj = new object();

        /// <summary>
        /// 日志队列
        /// </summary>
        private static Queue<LoggerEntity> m_LoggerQueue;

        /// <summary>
        /// 日志临时列表
        /// </summary>
        private static List<LoggerEntity> m_LoggerList;

        /// <summary>
        /// 上次保存时间
        /// </summary>
        private static DateTime m_PreSaveTime;

        /// <summary>
        /// 保存策略
        /// </summary>
        private static LinkedList<LoggerTactics> m_LoggerTactics;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            m_PreSaveTime = DateTime.UtcNow;
            m_LoggerQueue = new Queue<LoggerEntity>();
            m_LoggerList = new List<LoggerEntity>();
            m_LoggerTactics = new LinkedList<LoggerTactics>();

            // 队列>=1000条 立即写入
            m_LoggerTactics.AddLast(new LoggerTactics() { Count = 1000, Interval = 0 });

            // 队列>=100条 且 距离上次写入>=60秒写入
            m_LoggerTactics.AddLast(new LoggerTactics() { Count = 100, Interval = 60 });

            // 队列>=1条 且 距离上次写入>=300秒写入
            m_LoggerTactics.AddLast(new LoggerTactics() { Count = 1, Interval = 300 });
        }

        #region CurrClient
        private static MongoClient m_CurrClient = null;

        private static MongoClientSettings m_CurrClientSettings = new MongoClientSettings()
        {
            DirectConnection = true,
            Credential = MongoCredential.CreateCredential("admin", "admin", "123456"),
            Servers = new List<MongoServerAddress>() { new MongoServerAddress("114.67.240.38", 27017) },
            ApplicationName = "HHServer",
        };

        /// <summary>
        /// 当前的MongoClient
        /// </summary>
        public static MongoClient CurrClient
        {
            get
            {
                if (m_CurrClient == null)
                {
                    lock (lock_obj)
                    {
                        if (m_CurrClient == null)
                        {
                            m_CurrClient = new MongoClient(m_CurrClientSettings);
                        }
                    }
                }

                return m_CurrClient;
            }
        }

        #endregion

        #region LoggerDBModel

        private static LoggerDBModel m_LoggerDBModel = null;

        /// <summary>
        /// LoggerDBModel
        /// </summary>
        public static LoggerDBModel LoggerDBModel
        {
            get
            {
                if (m_LoggerDBModel == null)
                {
                    lock (lock_obj)
                    {
                        if (m_LoggerDBModel == null)
                        {
                            m_LoggerDBModel = new LoggerDBModel();
                        }
                    }
                }
                return m_LoggerDBModel;
            }
        }

        #endregion

        #region UniqueIDLogger

        private static UniqueIDLogger m_UniqueIDLogger = null;

        /// <summary>
        /// UniqueIDLogger
        /// </summary>
        public static UniqueIDLogger UniqueIDLogger
        {
            get
            {
                if (m_UniqueIDLogger == null)
                {
                    lock (lock_obj)
                    {
                        if (m_UniqueIDLogger == null)
                        {
                            m_UniqueIDLogger = new UniqueIDLogger();
                        }
                    }
                }
                return m_UniqueIDLogger;
            }
        }

        #endregion

        #region Log 记录日志

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="category">分类</param>
        /// <param name="message">消息体</param>
        /// <param name="args">参数</param>
        public static void Log(LoggerLevel level = LoggerLevel.Log, int category = 0, string message = null, params object[] args)
        {
            Log(level, category, true, message, args);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="category">分类</param>
        /// <param name="async">是否异步</param>
        /// <param name="message">消息体</param>
        /// <param name="args">参数</param>
        public static void Log(LoggerLevel level = LoggerLevel.Log, int category = 0, bool async = true, string message = null, params object[] args)
        {
            LoggerEntity loggerEntity = new LoggerEntity();
            loggerEntity.HFId = UniqueIDLogger.GetUniqueID(category);
            loggerEntity.Level = level;
            loggerEntity.Category = category;
            loggerEntity.Message = args.Length == 0 ? message : string.Format(message, args);
            loggerEntity.CreateTime = loggerEntity.UpdateTime = DateTime.UtcNow;

            Console.WriteLine($"{loggerEntity.Level} {loggerEntity.Category} {loggerEntity.Message}");

            // 加入队列
            m_LoggerQueue.Enqueue(loggerEntity);

            lock (lock_obj)
            {
                // 检查是否可以写入DB
                if (CheckCanSave())
                {
                    m_LoggerList.Clear();

                    while (m_LoggerQueue.Count > 0)
                    {
                        // 循环加入临时列表
                        m_LoggerList.Add(m_LoggerQueue.Dequeue());
                    }

                    if (async)
                    {
                        LoggerDBModel.AddManyAsync(m_LoggerList);
                    }
                    else
                    {
                        LoggerDBModel.AddMany(m_LoggerList);
                    }
                    m_PreSaveTime = DateTime.UtcNow;
                    Console.WriteLine("Logger写入DB完毕");
                }
            }
        }

        #endregion

        #region SaveDBWithStopServer 停服的时候写入DB

        /// <summary>
        /// 停服的时候写入DB
        /// </summary>
        public static void SaveDBWithStopServer()
        {
            m_LoggerList.Clear();

            while (m_LoggerList.Count > 0)
            {
                // 循环加入临时列表
                m_LoggerList.Add(m_LoggerQueue.Dequeue());
            }
            LoggerDBModel.AddMany(m_LoggerList);

            m_PreSaveTime = DateTime.UtcNow;
            Console.WriteLine("Logger 停服写入DB完毕");
        }

        #endregion

        #region CheckCanSave 检查释放可以写入DB

        /// <summary>
        /// 检查释放可以写入DB
        /// </summary>
        /// <returns></returns>
        private static bool CheckCanSave()
        {
            var curr = m_LoggerTactics.First;
            while (curr != null)
            {
                var loggerTactics = curr.Value;
                var interval = HFDateTimeUtil.GetTimestamp(DateTime.UtcNow) - HFDateTimeUtil.GetTimestamp(m_PreSaveTime); // 毫秒
                if (m_LoggerQueue.Count >= loggerTactics.Count && interval >= loggerTactics.Interval * 1000)
                {
                    return true;
                }
                curr = curr.Next;
            }
            return false;
        }

        #endregion
    }
}
