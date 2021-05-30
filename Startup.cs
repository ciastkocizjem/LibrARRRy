using Microsoft.Owin;
using Owin;
using Quartz;
using Quartz.Impl;
using System;
using System.IO;

[assembly: OwinStartupAttribute(typeof(LibrARRRy.Startup))]
namespace LibrARRRy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureApplyingPenaltyAsync();
        }

        public async System.Threading.Tasks.Task ConfigureApplyingPenaltyAsync()
        {
            // Configure job to check for loans past due
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<ApplyPenaltyJob>().Build();
            //ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInHours(24)).Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(30)).Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
