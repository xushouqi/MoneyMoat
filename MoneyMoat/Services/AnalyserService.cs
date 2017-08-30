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

namespace MoneyMoat.Services
{
     class AnalyserService
    {
        private readonly ILogger m_logger;
        protected readonly IBClient ibClient;
        private readonly IRepository<Stock> m_repoStock;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<Recommendation> m_repoRecommend;
        private readonly IRepository<XueQiuData> m_repoDatas;
        private readonly FundamentalService m_fundamentalService;
        private readonly HistoricalService m_historicalService;

        public AnalyserService(IBClient ibclient,
                        IRepository<Stock> repoStock,
                        IRepository<Financal> repoFin,
                        IRepository<FYEstimate> repoEst,
                        IRepository<NPEstimate> repoNPE,
                        IRepository<Recommendation> repoRecommend,
                        IRepository<XueQiuData> repoDatas,
                        FundamentalService fundamentalService,
                        HistoricalService historicalService,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            m_repoStock = repoStock;
            m_repoFin = repoFin;
            m_repoEst = repoEst;
            m_repoNPE = repoNPE;
            m_repoRecommend = repoRecommend;
            m_repoDatas = repoDatas;
            m_fundamentalService = fundamentalService;
            m_historicalService = historicalService;

        }

        public async Task UpdateDatas(string symbol)
        {
            var stock = m_repoStock.Find(symbol);

            //更新基本面数据
            await m_fundamentalService.UpdateFundamentalsFromIB(symbol, stock.Exchange, !ibClient.ClientSocket.IsConnected());
            //更新历史数据
            await m_historicalService.UpdateHistoricalDataFromXueQiu(symbol);
        }

        public async Task AnalyseStock(string symbol)
        {
            var stock = m_repoStock.Find(symbol);


        }

    }
}
