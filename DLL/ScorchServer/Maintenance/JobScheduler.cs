using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorchServer.Maintenance
{
    using Quartz;
    using Quartz.Impl;

    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<GameCleanupJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(s =>
                     s.WithIntervalInMinutes(10)
                     .RepeatForever()
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}