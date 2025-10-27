
using Jim.Quartz.Provider;
using Microsoft.Extensions.Hosting;

namespace Jim.Quartz
{
    public class QuartzHostService : IHostedService
    {
        private readonly ITaskJobManage _taskJobManage;
        public QuartzHostService(ITaskJobManage taskJobManage)
        {
            _taskJobManage = taskJobManage;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("TaskJobWork.StartAsync...");
            // 启动任务
            return _taskJobManage.StartUp();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("TaskJobWork.StopAsync...");
            // 停止任务
            return _taskJobManage.Shutdown();
        }
    }
}
