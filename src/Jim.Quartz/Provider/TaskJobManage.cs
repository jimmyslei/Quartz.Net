using Jim.Quartz.Provider;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System.Reflection;

namespace Jim.Quartz
{
    public class TaskJobManage : ITaskJobManage
    {
       
        /// <summary>
        /// 调度器实例
        /// </summary>
        private static IScheduler _scheduler;

        public async Task<bool> StartUp()
        {
            try
            {
                if (_scheduler == null || _scheduler.IsShutdown)
                {
                    _scheduler = await new StdSchedulerFactory().GetScheduler();
                }

                await _scheduler.Start();
                await _scheduler.Clear();

                return true;
            }
            catch (Exception ex)
            {
                throw new SchedulerException("任务调度平台初始化失败！", ex);
            }
        }

        private static async void TaskJobListener_OnSuccess(string scheduleId)
        {
            // 任务后续操作
            await Task.CompletedTask;
        }

        public async Task Shutdown()
        {
            try
            {
                // 判断调度是否已经关闭
                if (_scheduler != null && !_scheduler.IsShutdown)
                {
                    // 等待任务运行完成再关闭调度
                    await _scheduler.Clear();
                    await _scheduler.Shutdown(true);

                    _scheduler = null;
                }
            }
            catch (Exception ex)
            {
                throw new SchedulerException("任务调度平台停止失败！", ex);
            }
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="model">执行参数</param>
        /// <returns></returns>
        /// <exception cref="SchedulerException"></exception>
        public async Task Start(ExecuteModel model)
        {
            if (_scheduler == null) return;

            JobDataMap map = new JobDataMap
            {
                new KeyValuePair<string, object> ("FunctionDelegate", model.Function)
            };

            try
            {
                IJobDetail job = JobBuilder.Create<Job>()
                .WithIdentity(model.ScheduleId)
                .UsingJobData(map)
                .Build();

                // 监听器
                var listener = new JobRunListener(model.ScheduleId);
                listener.OnSuccess += TaskJobListener_OnSuccess;
                _scheduler.ListenerManager.AddJobListener(listener, KeyMatcher<JobKey>.KeyEquals(new JobKey(model.ScheduleId)));

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(model.ScheduleId)
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(model.StartAt)); // {}秒后执行
                if (model.ExecuteType == ExecuteType.Once)
                {
                    trigger.WithSimpleSchedule(o =>
                    {
                        o.WithInterval(TimeSpan.FromSeconds(model.Time)).WithRepeatCount(0); // 只执行一次
                    });
                }
                else if (model.ExecuteType == ExecuteType.Repeat)
                {
                    trigger.WithSimpleSchedule(o =>
                    {
                        o.WithIntervalInSeconds(Convert.ToInt32(model.Time)).RepeatForever(); // 每{}秒执行一次
                    });
                }
                else
                {
                    trigger.WithCronSchedule(model.Cron);
                }


                ITrigger triggerBuild = trigger.Build();
                await _scheduler.ScheduleJob(job, triggerBuild);
            }
            catch (Exception ex)
            {
                throw new SchedulerException(ex);
            }
        }


        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        public async Task<bool> Stop(string scheduleId)
        {
            if (_scheduler == null) return false;

            JobKey jk = new JobKey(scheduleId);

            var job = await _scheduler.GetJobDetail(jk);
            if (job != null)
            {
                var trigger = new TriggerKey(scheduleId);
                await _scheduler.PauseTrigger(trigger);
                await _scheduler.UnscheduleJob(trigger);
                await _scheduler.DeleteJob(jk);
            }

            return true;
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        public async Task<bool> Pause(string scheduleId)
        {
            if (_scheduler == null) return false;

            JobKey jk = new JobKey(scheduleId);
            if (await _scheduler.CheckExists(jk))
            {
                await _scheduler.PauseJob(jk);

                return true;
            }
            else
            {
                throw new SchedulerException("任务不存在！");
            }
        }

        /// <summary>
        /// 恢复运行
        /// </summary>
        /// <param name="scheduleId">定时器id</param>
        /// <returns></returns>
        public async Task<bool> Resume(string scheduleId)
        {
            if (_scheduler == null) return false;

            JobKey jk = new JobKey(scheduleId);
            if (await _scheduler.CheckExists(jk))
            {
                await _scheduler.ResumeJob(jk);

                return true;
            }
            else
            {
                throw new SchedulerException("任务不存在！");
            }
        }

    }
}
