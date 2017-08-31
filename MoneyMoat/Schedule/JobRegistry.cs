using FluentScheduler;
using System;
using MoneyMoat.Services;

namespace MoneyMoat
{
    public class JobRegistry : Registry
    {
        public JobRegistry(IServiceProvider services)
        {
            NonReentrantAsDefault();

            var ibManager = (IBManager)services.GetService(typeof(IBManager));
            ibManager.Connect();

            //更新历史报价数据：每天收盘后
            Schedule(() => new HistoricalJob(services)).ToRunEvery(0).Days().At(8, 30);

            //更新基本面数据
            Schedule(() => new FundamentalJob(services)).ToRunEvery(0).Days().At(12, 30);

            // Schedule an IJob to run at an interval
            //Schedule(() => new JobService(wService)).ToRunEvery(0).Days().At(hour, minute);

            //// Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

            //// Schedule a simple job to run at a specific time
            //Schedule(() => Console.WriteLine("It's 9:15 PM now.")).ToRunEvery(1).Days().At(21, 15);

            //// Schedule a more complex action to run immediately and on an monthly interval
            //Schedule<MyComplexJob>().ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            //// Schedule a job using a factory method and pass parameters to the constructor.
            //Schedule(() => new MyComplexJob("Foo", DateTime.Now)).ToRunNow().AndEvery(2).Seconds();

            //// Schedule multiple jobs to be run in a single schedule
            //Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}