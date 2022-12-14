namespace HHServer.Core
{
    /// <summary>
    /// 日志等级
    /// </summary>
    public enum LoggerLevel
    {
        /// <summary>
        /// 普通日志
        /// </summary>
        Log = 0,
        /// <summary>
        /// 警告日志
        /// </summary>
        LogWarning = 1,
        /// <summary>
        /// 错误日志
        /// </summary>
        LogError = 2
    }

    /// <summary>
    /// 数据状态枚举
    /// </summary>
    public enum DataStatus
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 已删除
        /// </summary>
        Delete = 1
    }
}
