using System;
using System.Collections.Generic;
using System.Text;

namespace HHServer.Model.Logic.Entitys
{
    /// <summary>
    /// 任务实体
    /// </summary>
    public class TaskEntity
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int TaskId;
        /// <summary>
        /// 当前状态
        /// </summary>
        public byte CurrStatus;
    }
}
