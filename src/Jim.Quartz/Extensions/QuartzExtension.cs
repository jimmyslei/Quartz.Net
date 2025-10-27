using Jim.Quartz.Provider;
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
        services.AddHostedService<QuartzHostService>();
        return services;
    }
}
#if NETSTANDARD2_0
}
#endif
