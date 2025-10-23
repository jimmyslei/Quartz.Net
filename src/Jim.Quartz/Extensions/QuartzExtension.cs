using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !NETSTANDARD2_0
namespace Jim.Quartz;
#else
namespace Jim.Quartz {
#endif
public static class QuartzExtensions
{
    /// <summary>
    /// 注册redis服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
       
        return services;
    }
}
#if NETSTANDARD2_0
}
#endif
