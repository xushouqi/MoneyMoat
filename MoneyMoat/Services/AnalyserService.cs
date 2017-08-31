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
using IBApi;
using YAXLib;
using CommonLibs;

namespace MoneyMoat.Services
{
    [WebApi]
    public class AnalyserService
    {
        private readonly ILogger m_logger;
        protected readonly IBClient ibClient;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<Recommendation> m_repoRecommend;
        private readonly IRepository<XueQiuData> m_repoDatas;
        private readonly SymbolService m_symbolService;
        private readonly FundamentalService m_fundamentalService;
        private readonly HistoricalService m_historicalService;

        IServiceProvider _services;
        public AnalyserService(IBClient ibclient,
                        IServiceProvider services,
                        SymbolService symbolService,
                        //IRepository<Financal> repoFin,
                        //IRepository<FYEstimate> repoEst,
                        //IRepository<NPEstimate> repoNPE,
                        //IRepository<Recommendation> repoRecommend,
                        //IRepository<XueQiuData> repoDatas,
                        //FundamentalService fundamentalService,
                        //HistoricalService historicalService,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;
            _services = services;
            m_symbolService = symbolService;

            //m_repoFin = repoFin;
            //m_repoEst = repoEst;
            //m_repoNPE = repoNPE;
            //m_repoRecommend = repoRecommend;
            //m_repoDatas = repoDatas;
            //m_fundamentalService = fundamentalService;
            //m_historicalService = historicalService;

        }

        public async Task UpdateDatas(string symbol)
        {
            //更新基本面数据
            await m_fundamentalService.UpdateAllFromIB(symbol);
            //更新历史数据
            await m_historicalService.UpdateHistoricalDataFromXueQiu(symbol);
        }

        public async Task AnalyseStock(string symbol)
        {
            var stock = await m_symbolService.FindAsync(symbol);


        }

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
