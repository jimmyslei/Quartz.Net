using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim.Quartz
{
    public class ExecuteModel
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string ScheduleId { get; set; }
        /// <summary>
        /// 任务开始时间(秒)
        /// </summary>
        public double StartAt { get; set; }
        /// <summary>
        /// 执行周期(秒)
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// 任务执行类型
        /// </summary>
        public ExecuteType ExecuteType { get; set; }
        /// <summary>
        /// Cron表达式
        /// 当ExecuteType为Cron时，必须填写此项
        /// </summary>
        public string Cron { get; set; }
        /// <summary>
        /// 需要执行的函数
        /// </summary>
        public FunctionDelegate Function { get; set; }

        //public Func<Task> Function { get; set; }

    }
}
