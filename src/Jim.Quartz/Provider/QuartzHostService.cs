
using Jim.Quartz.Provider;
using Microsoft.Extensions.Hosting;

namespace Jim.Quartz
{
    public class QuartzHostService : IHostedService
    {
        private readonly ITaskJobManage _taskJobManage;
        private List<ExecuteModel>? _executeModes;

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
            if (_executeModes != null && _taskJobManage is TaskJobManage taskJobManage)
            {
                taskJobManage.SetImmediateJob(_executeModes);
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
        public void SetImmediateJob(List<ExecuteModel> executeModels)
        {
            _executeModes = executeModels;

            // 如果TaskJobManage已经启动，直接设置立即执行的任务
            if (_taskJobManage is TaskJobManage taskJobManage)
            {
                taskJobManage.SetImmediateJob(executeModels);
            }
        }
    }
}
