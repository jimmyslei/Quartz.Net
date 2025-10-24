using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim.Quartz
{
    public enum ExecuteType
    {
        /// <summary>
        /// 执行一次
        /// </summary>
        Once,
        /// <summary>
        /// 重复执行
        /// </summary>
        Repeat,
        /// <summary>
        /// cron表达式执行
        /// </summary>
        Cron,

    }
}
