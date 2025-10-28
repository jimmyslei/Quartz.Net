
using Jim.Quartz.Provider;
using Microsoft.Extensions.Hosting;

namespace Jim.Quartz
{
    public class QuartzHostService : IHostedService
    {
        private readonly ITaskJobManage _taskJobManage;
        private ExecuteModel? _executeMode;

        public QuartzHostService(ITaskJobManage taskJobManage)
        {
            _taskJobManage = taskJobManage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("TaskJobWork.StartAsync...");
            // 启动任务
            var result = _taskJobManage.StartUp();

            // 如果设置了立即执行的任务，并且TaskJobManage支持立即执行
            if (_executeMode != null && _taskJobManage is TaskJobManage taskJobManage)
            {
                taskJobManage.SetImmediateJob(_executeMode);
            }

            return result;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("TaskJobWork.StopAsync...");
            // 停止任务
            return _taskJobManage.Shutdown();
        }
        
        /// <summary>
        /// 设置立即执行的任务
        /// </summary>
        /// <param name="immediateJobDelegate">立即执行的任务委托</param>
        public void SetImmediateJob(ExecuteModel executeModel)
        {
            _executeMode = executeModel;

            // 如果TaskJobManage已经启动，直接设置立即执行的任务
            if (_taskJobManage is TaskJobManage taskJobManage)
            {
                taskJobManage.SetImmediateJob(executeModel);
            }
        }
    }
}
