using Jim.Quartz;
using Jim.Quartz.Provider;
using Microsoft.AspNetCore.Mvc;

namespace Jim.QuartzDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ITaskJobManage _taskJobManage;


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,ITaskJobManage taskJobManage)
        {
            _logger = logger;
            _taskJobManage = taskJobManage;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var model = new ExecuteModel
            {
                ScheduleId = Guid.NewGuid().ToString(),
                StartAt = 5, // 5秒后执行
                ExecuteType = ExecuteType.Repeat,
                Time = 5,
                Function = HelloWork
            };
            // 启动定时任务
            _taskJobManage.Start(model);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        private async Task HelloWork()
        {
            Console.WriteLine("Hello JIm.Quartz.NET World!");
            await Task.CompletedTask;
        }

    }
}
