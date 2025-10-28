# Jim.Quartz

基于 Quartz.NET 封装的轻量级任务调度框架，简化任务调度的使用流程。

## 功能特性

- ✅ 支持三种任务执行类型：一次性执行、重复执行、Cron 表达式执行
- ✅ 简单易用的 API 设计
- ✅ 集成 ASP.NET Core 依赖注入
- ✅ 支持任务启动、停止、暂停、恢复操作
- ✅ 支持应用启动后立即执行任务
- ✅ 任务执行监听器支持
- ✅ 作为后台服务自动启动和关闭

## 安装

```bash
dotnet add package Jim.Quartz
```

或在 `.csproj` 文件中添加：

```xml
<ItemGroup>
  <PackageReference Include="Jim.Quartz" Version="1.0.0" />
</ItemGroup>
```

## 快速开始

### 1. 在 Program.cs 中注册服务

```csharp
using Jim.Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// 注册 Quartz 服务
builder.Services.AddJimQuartz();

var app = builder.Build();

// 可选：设置应用启动后立即执行的任务
app.UseJimQuartz(async () => {
    Console.WriteLine("应用启动后立即执行的任务");
    await Task.CompletedTask;
});

app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 2. 注入 ITaskJobManage 并使用

```csharp
using Jim.Quartz;
using Jim.Quartz.Provider;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskJobManage _taskJobManage;

        public TaskController(ITaskJobManage taskJobManage)
        {
            _taskJobManage = taskJobManage;
        }

        [HttpPost("start")]
        public async Task StartTask()
        {
            var model = new ExecuteModel
            {
                ScheduleId = Guid.NewGuid().ToString(), // 唯一任务ID
                StartAt = 5,                            // 5秒后开始执行
                ExecuteType = ExecuteType.Repeat,       // 重复执行
                Time = 10,                              // 每10秒执行一次
                Function = MyTask                       // 要执行的函数
            };

            await _taskJobManage.Start(model);
        }

        private async Task MyTask()
        {
            Console.WriteLine("任务执行中...");
            // 在这里编写你的任务逻辑
            await Task.CompletedTask;
        }
    }
}
```

## API 文档

### ExecuteModel（任务模型）

```csharp
public class ExecuteModel
{
    /// <summary>
    /// 任务唯一标识
    /// </summary>
    public string ScheduleId { get; set; }

    /// <summary>
    /// 任务开始延迟时间（秒）
    /// </summary>
    public double StartAt { get; set; }

    /// <summary>
    /// 任务执行周期（秒）
    /// </summary>
    public double Time { get; set; }

    /// <summary>
    /// 任务执行类型
    /// </summary>
    public ExecuteType ExecuteType { get; set; }

    /// <summary>
    /// Cron 表达式（ExecuteType 为 Cron 时必填）
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 需要执行的委托函数
    /// </summary>
    public FunctionDelegate Function { get; set; }
}
```

### ExecuteType（执行类型）

```csharp
public enum ExecuteType
{
    /// <summary>
    /// 执行一次
    /// </summary>
    Once,

    /// <summary>
    /// 重复执行
    /// </summary>
    Repeat,

    /// <summary>
    /// Cron 表达式执行
    /// </summary>
    Cron
}
```

### ITaskJobManage（任务管理接口）

```csharp
public interface ITaskJobManage
{
    /// <summary>
    /// 启动任务
    /// </summary>
    Task Start(ExecuteModel model);

    /// <summary>
    /// 停止任务
    /// </summary>
    Task<bool> Stop(string scheduleId);

    /// <summary>
    /// 暂停任务
    /// </summary>
    Task<bool> Pause(string scheduleId);

    /// <summary>
    /// 恢复运行
    /// </summary>
    Task<bool> Resume(string scheduleId);
}
```

## 使用示例

### 示例 1：一次性任务

```csharp
var model = new ExecuteModel
{
    ScheduleId = "task-once-001",
    StartAt = 3,                      // 3秒后执行
    ExecuteType = ExecuteType.Once,
    Time = 0,                         // 只执行一次，Time 值不重要
    Function = async () => {
        Console.WriteLine("执行一次性任务");
        await Task.CompletedTask;
    }
};

await _taskJobManage.Start(model);
```

### 示例 2：重复执行任务

```csharp
var model = new ExecuteModel
{
    ScheduleId = "task-repeat-001",
    StartAt = 5,                      // 5秒后开始
    ExecuteType = ExecuteType.Repeat,
    Time = 30,                        // 每30秒执行一次
    Function = async () => {
        Console.WriteLine($"定时任务执行 - {DateTime.Now}");
        await Task.CompletedTask;
    }
};

await _taskJobManage.Start(model);
```

### 示例 3：Cron 表达式任务

```csharp
var model = new ExecuteModel
{
    ScheduleId = "task-cron-001",
    StartAt = 0,                      // 立即开始
    ExecuteType = ExecuteType.Cron,
    Cron = "0 0 12 * * ?",           // 每天中午12点执行
    Function = async () => {
        Console.WriteLine("每天定时执行");
        await Task.CompletedTask;
    }
};

await _taskJobManage.Start(model);
```

### 示例 4：应用启动后立即执行任务

```csharp
// 在 Program.cs 中设置应用启动后立即执行的任务
app.UseJimQuartz(async () => {
    Console.WriteLine("应用启动后立即执行的任务");
    // 可以执行初始化操作、数据预热等
    await Task.CompletedTask;
});
```

### 示例 5：任务管理操作

```csharp
// 暂停任务
await _taskJobManage.Pause("task-001");

// 恢复任务
await _taskJobManage.Resume("task-001");

// 停止任务
await _taskJobManage.Stop("task-001");
```

## Cron 表达式示例

| Cron 表达式          | 说明                                      |
| -------------------- | ----------------------------------------- |
| `0 0 12 * * ?`       | 每天中午 12 点执行                        |
| `0 0/5 14 * * ?`     | 每天下午 14 点到 14:59，每 5 分钟执行一次 |
| `0 0 10 ? * MON`     | 每周一上午 10 点执行                      |
| `0 0 0 1 * ?`        | 每月 1 号凌晨执行                         |
| `0 0 12 ? * WED,FRI` | 每周三和周五中午 12 点执行                |
| `0 0-5 14 * * ?`     | 每天下午 14:00 到 14:05，每分钟执行一次   |

## 常见 Cron 字段说明

```text
秒 分 时 日 月 星期
* * * * * *
```

- **秒 (0-59)**
- **分 (0-59)**
- **时 (0-23)**
- **日 (1-31)**
- **月 (1-12 或 JAN-DEC)**
- **星期 (0-6 或 SUN-SAT，0 和 7 都代表星期日)**

## 注意事项

1. `ScheduleId` 必须唯一，建议使用 `Guid.NewGuid().ToString()` 生成
2. 使用 Cron 表达式时，必须设置 `ExecuteType` 为 `Cron` 并填写 `Cron` 字段
3. `Function` 委托必须是异步函数 (`async Task`)
4. 任务会在应用程序启动时自动初始化，关闭时自动停止

## 依赖项

- .NET 8.0 / .NET 9.0
- Quartz.Extensions.Hosting (3.15.1)
- Microsoft.Extensions.DependencyInjection.Abstractions (9.0.10)

## 许可证

MIT License

## 贡献

欢迎提交 Issue 和 Pull Request！
