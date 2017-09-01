using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using MoneyMoat.Types;
using MoneyModels;
using YAXLib;
using CommonLibs;
using Polly;
using Polly.Bulkhead;

namespace MoneyMoat.Services
{
    [WebApi]
    public class AnalyserService
    {
        private readonly ILogger m_logger;
        private readonly SymbolService m_symbolService;

        private readonly IServiceProvider _services;

        public AnalyserService(IServiceProvider services,
                        SymbolService symbolService,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            _services = services;
            m_symbolService = symbolService;
        }

        public async Task AnalyseStock(string symbol)
        {
            var stock = await m_symbolService.FindAsync(symbol);
        }

        /// <summary>
        /// 更新所有基本面数据
        /// </summary>
        /// <param name="interval">启动间隔</param>
        /// <param name="count">同时启动数量</param>
        /// <returns></returns>
        [Api]
        public async Task<int> UpdateAllFundamentals(int interval = 5, int count = 10)
        {
            if (interval < 1)
                interval = 1;
            if (count < 1)
                count = 1;

            List<Task> tasklist = new List<Task>();
            var datalist = await m_symbolService.GetAllAsync();
            for (int i = 0; i < datalist.Count; i++)
            {
                var stock = datalist[i];
                var fundamentalService = (FundamentalService)_services.GetService(typeof(FundamentalService));
                Task task = fundamentalService.UpdateAllFromIB(stock);
                tasklist.Add(task);
                await Task.Delay(interval);

                if (tasklist.Count >= count)
                {
                    int idx = Task.WaitAny(tasklist.ToArray());
                    tasklist.RemoveAt(idx);
                }
            }
            return datalist.Count;
        }

        /// <summary>
        /// 更新所有历史报价
        /// </summary>
        /// <param name="interval">启动间隔</param>
        /// <param name="count">同时启动数量</param>
        /// <returns></returns>
        [Api]
        public async Task<int> UpdateAllHistoricals(int interval = 5, int count = 10)
        {
            if (interval < 1)
                interval = 1;
            if (count < 1)
                count = 1;

            List<Task> tasklist = new List<Task>();
            var datalist = await m_symbolService.GetAllAsync();
            for (int i = 0; i < datalist.Count; i++)
            {
                var stock = datalist[i];
                var historicalService = (HistoricalService)_services.GetService(typeof(HistoricalService));
                Task task = historicalService.UpdateHistoricalDataFromXueQiu(stock.Symbol);
                tasklist.Add(task);
                await Task.Delay(interval);

                if (tasklist.Count >= count)
                {
                    int idx = Task.WaitAny(tasklist.ToArray());
                    tasklist.RemoveAt(idx);
                }
            }
            return datalist.Count;
        }
    }
}
