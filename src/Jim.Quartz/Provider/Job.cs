

using Quartz;

namespace Jim.Quartz
{
    /// <summary>
    /// 委托函数
    /// </summary>
    /// <returns></returns>
    public delegate Task FunctionDelegate();

    public class Job : IJob
    {
        public FunctionDelegate FunctionDelegate { get; set; }
        public async Task Execute(IJobExecutionContext context)
        {
            await FunctionDelegate();

        }
    }
}
