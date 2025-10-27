using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jim.Quartz.Provider
{
    public interface ITaskJobManage
    {
        Task<bool> StartUp();

        Task Shutdown();

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="model">执行的参数</param>
        /// <returns></returns>
        /// <exception cref="SchedulerException"></exception>
        Task Start(ExecuteModel model);


        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        Task<bool> Stop(string scheduleId);

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        Task<bool> Pause(string scheduleId);

        /// <summary>
        /// 恢复运行
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        Task<bool> Resume(string scheduleId);
    }
}
