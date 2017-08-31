using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using MoneyMoat.Services;

namespace MoneyMoat
{
    public class FundamentalJob : IJob
    {
        private readonly IServiceProvider _services;
        private readonly object _lock = new object();

        private bool _shuttingDown = false;

        public FundamentalJob(IServiceProvider services)
        {
            _services = services;
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;

                var ibManager = (IBManager)_services.GetService(typeof(IBManager));      
                ibManager.Connect();

                var fundamentalService = (FundamentalService)_services.GetService(typeof(FundamentalService));
                fundamentalService.UpdateAllStocks().Wait();
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }
        }
    }
}
