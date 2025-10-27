
using Quartz;

namespace Jim.Quartz
{
    public class JobRunListener : IJobListener
    {
        public delegate void SuccessEventHandler(string scheduleId);
        public event SuccessEventHandler OnSuccess;

        public string Name { get; set; }

        public JobRunListener(string name)
        {
            this.Name = name;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            if (jobException == null)
            {
                IJobDetail job = context.JobDetail;

                //this.OnSuccess?.Invoke(JSON.Serialize(job.JobDataMap["scheduleInfo"]));
                this.OnSuccess?.Invoke(job.Key.Name);
            }

            return Task.FromResult(0);
        }
    }
}
