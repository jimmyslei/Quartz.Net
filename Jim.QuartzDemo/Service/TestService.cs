using Jim.Quartz;

namespace Jim.QuartzDemo.Service
{
    public class TestService: ITestService
    {
        public ExecuteModel TestLog()
        {
            return new ExecuteModel
            {
                ScheduleId = "TestLogJob",
                StartAt = 5,
                Time = 5,
                ExecuteType = ExecuteType.Repeat,
                Function = FuncLog
            };
        }

        public async Task FuncLog()
        {
            Console.WriteLine("=============》Execute Test Log 《=============");
            await Task.CompletedTask;
        }
    }
}
