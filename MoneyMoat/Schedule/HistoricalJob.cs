﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using MoneyMoat.Services;

namespace MoneyMoat
{
    class HistoricalJob : IJob
    {
        private readonly IServiceProvider _services;
        private readonly object _lock = new object();

        private bool _shuttingDown = false;

        //private readonly string Symbol;

        public HistoricalJob(IServiceProvider services)
        {
            _services = services;
            //Symbol = symbol;
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;

                var historicalService = (HistoricalService)_services.GetService(typeof(HistoricalService));
                historicalService.UpdateAllStocks().Wait();
                //historicalService.UpdateHistoricalDataFromXueQiu(Symbol).Wait();
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
