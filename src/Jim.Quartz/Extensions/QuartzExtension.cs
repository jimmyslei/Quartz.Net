using Jim.Quartz.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


#if !NETSTANDARD2_0
namespace Jim.Quartz;
#else
namespace Jim.Quartz {
#endif
public static class QuartzExtensions
{
    /// <summary>
    /// 注册Quartz服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddJimQuartz(this IServiceCollection services)
    {
        services.TryAddSingleton<ITaskJobManage, TaskJobManage>();
        services.AddSingleton<QuartzHostService>();
        services.AddHostedService(sp => sp.GetRequiredService<QuartzHostService>());
        return services;
    }

    /// <summary>
    /// 设置启动定时器执行任务
    /// </summary>
    /// <param name="app">应用程序构建器</param>
    /// <param name="functionJobDelegate">委托函数</param>
    /// <returns>应用程序构建器</returns>
    public static IApplicationBuilder UseJimQuart(this IApplicationBuilder app, ExecuteModel executeModel)
    {
        var quartzHostService = app.ApplicationServices.GetRequiredService<QuartzHostService>();
        quartzHostService.SetImmediateJob(executeModel);
        return app;
    }
}
#if NETSTANDARD2_0
}
#endif
