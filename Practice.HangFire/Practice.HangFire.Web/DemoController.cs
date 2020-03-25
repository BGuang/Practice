using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Practice.HangFire.Web
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DemoController : Controller
    {

        #region BackgroundJob
        [HttpGet]
        public string One()
        {
            //基于任务队列

            //任务执行一次
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine($"OneTime:{DateTime.Now}"));
            return "OK," + jobId;
        }
        [HttpGet]
        public string DelayDemo()
        {
            //延时任务

            //任务延时两分钟执行
            Hangfire.BackgroundJob.Schedule(() => Console.WriteLine($"Delay two minute:{DateTime.Now}"), TimeSpan.FromMinutes(2));
            return "OK";
        }
        [HttpGet]
        public string ContinueJobDemo()
        {
            //连续执行任务
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Delay two minute:{DateTime.Now}"), TimeSpan.FromMinutes(2));

            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation!"));
            return "OK";
        }
        #endregion


        [HttpGet]
        public string CronDemo()
        {
            //Cron方式
            //任务每分钟执行一次
            RecurringJob.AddOrUpdate(() => Console.WriteLine($"Every minute:{DateTime.Now}"), Cron.Minutely());
            return "OK";

        }

        public string BatchJobDemo()
        {
            //PRO 版本 支持
            //var batchId =Batch.StartNew(x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Job 1"));
            //    x.Enqueue(() => Console.WriteLine("Job 2"));
            //});

            //Batch.ContinueWith(batchId, x =>
            //{
            //    x.Enqueue(() => Console.WriteLine("Last Job"));
            //});
            return "OK";
        }

    }
}
