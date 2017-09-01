using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using IBApi;
using MoneyModels;
using MoneyMoat.Types;
using MoneyMoat.Messages;

namespace MoneyMoat.Services
{
    public class TestService
    {
        private readonly ILogger m_logger;
        private readonly AppSettings m_config;

        protected readonly SymbolService m_symbolSampleService;
        protected readonly FundamentalService m_fundamentalService;
        protected readonly AccountService m_accountService;
        protected readonly HistoricalService m_historicalService;
        protected readonly ScannerService m_scannerService;

        public TestService(
                        SymbolService symbolService,
                        FundamentalService fundamentalService,
                        AccountService accountService,
                        HistoricalService historicalService,
                        ScannerService scannerService,
                        ILogger<IBManager> logger,
                        IOptions<AppSettings> commonoptions)
        {
            m_logger = logger;
            m_config = commonoptions.Value;
            m_symbolSampleService = symbolService;
            m_fundamentalService = fundamentalService;
            m_accountService = accountService;
            m_historicalService = historicalService;
            m_scannerService = scannerService;
        }

        public async Task Work()
        {
            await m_historicalService.UpdateAllStocks();
            await m_fundamentalService.UpdateAllStocks();
            //m_fundamentalService.RequestAndParseFundamentalsAsync("ATAI", ExchangeEnum.ISLAND.ToString(), FundamentalsReportEnum.RESC).Wait();
            //m_fundamentalService.DoUpdateStockFundamentals("ATAI", ExchangeEnum.ISLAND.ToString()).Wait();
            //m_fundamentalService.UpdateAllStocksFundamentals().Wait();
            //m_symbolSampleService.UpdateSymbolsFromSina().Wait();
            //m_historicalService.UpdateAllStocks().Wait();
            //m_fundamentalService.UpdateAllStocks().Wait();

            //RequestAccountSummery();
            //m_symbolSampleService.RequestSymbols("601011");
            //m_historicalService.RequestEarliestDataPoint("601011", ExchangeEnum.SHSE);
            //m_fundamentalService.RequestFundamentals("BABA", ExchangeEnum.NYSE, FundamentalsReportEnum.ReportsFinSummary);
            //m_fundamentalService.RequestFundamentals("601011", ExchangeEnum.SHSE, FundamentalsReportEnum.ReportsFinStatements);

            //m_scannerService.RequestParameters();
            //m_scannerService.AddRequest(ScanCodeEnum.HIGH_VS_13W_HL, SecTypeEnum.STK, StockTypeFilterEnum.ALL, 10);

            //DateTime endTime = DateTime.Now;
            //m_historicalService.AddRequest("MOMO", ExchangeEnum.ISLAND, endTime, "3 W", "1 day", WhatToShowEnum.MIDPOINT, false);
        }


    }
}
