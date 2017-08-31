using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using IBApi;
using AngleSharp.Parser.Html;
using MoneyModels;
using CommonLibs;

namespace MoneyMoat.Services
{
    [WebApi]
    public class SymbolService : IBServiceBase<ContractDescription[]>
    {
        private readonly ILogger m_logger;
        private readonly HistoricalService m_historicalService;
        private readonly IRepository<Stock> m_repoStock;

        public SymbolService(IBManager ibmanager,
                        HistoricalService historicalService,
                        IRepository<Stock> repoStock,
                        ILogger<IBManager> logger) : base(ibmanager)
        {
            m_logger = logger;
            m_historicalService = historicalService;
            m_repoStock = repoStock;

            ibClient.SymbolSamples += HandleSymbolSamplesData;

            m_categories.Add("Chinese");
            m_categories.Add("Technology");
            m_categories.Add("Finance");
            m_categories.Add("FDA");
            m_categories.Add("Media");
            m_categories.Add("Energy");
            m_categories.Add("Manufacture");
            m_categories.Add("ETF");
        }

        List<string> m_categories = new List<string>();

        public async Task UpdateSymbolsFromSina()
        {            
            var parser = new HtmlParser();

            var url = "http://vip.stock.finance.sina.com.cn/usstock/ustotal.php";
            var source = await MoatCommon.GetHttpContent(url, System.Text.Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(source))
            {            
                var document = parser.Parse(source);

                var blocklist = document.All.Where(t => t.ClassName == "col_div").ToArray();
                for (int i = 0; i < blocklist.Length; i++)
                {
                    var body = blocklist[i];
                    var hrefs = body.GetElementsByTagName("a");
                    for (int j = 0; j < hrefs.Length; j++)
                    {
                        var node = hrefs[j];
                        
                        var match = Regex.Match(node.InnerHtml, @"(?<=\().*?(?=\))");
                        if (match.Success)
                        {
                            var symbol = match.Value;
                            var name = node.InnerHtml.Replace("(" + symbol + ")", "");
                            Console.WriteLine("{0}: {1}", symbol, name);

                            await UpdateStock(name, symbol, m_categories[i], false);
                        }
                    }
                }
                await m_repoStock.SaveChangesAsync();
            }
        }

        [Api]
        public async Task<Stock> FindAsync(string symbol)
        {
            Stock data = await UpdateStock(symbol, symbol, "", true);
            return data;
        }

        [Api]
        public async Task<List<Stock>> GetAllAsync()
        {
            return await m_repoStock.GetAllAsync();
        }
        public async Task Update(Stock data)
        {
            m_repoStock.Update(data);
            await m_repoStock.SaveChangesAsync();
        }

        [Api]
        public async Task<Stock> UpdateStock(string name, string symbol, string category, bool saveToDb)
        {
            Stock data = m_repoStock.Find(symbol);
            if (data == null)
            {
                var resuls = await SearchSymbolsAsync(symbol);
                if (resuls != null && resuls.Length > 0)
                {
                    m_logger.LogError("SearchSymbolsAsync, count={0}", resuls.Length);

                    for (int c = 0; c < resuls.Length; c++)
                    {
                        var detail = resuls[c];
                        if (symbol.Equals(detail.Contract.Symbol))
                        {
                            try
                            {
                                var exchange = detail.Contract.PrimaryExch;
                                if (exchange.Contains("NASDAQ"))
                                    exchange = ExchangeEnum.ISLAND.ToString();

                                if (exchange.Equals(ExchangeEnum.ISLAND.ToString()) || exchange.Equals(ExchangeEnum.NYSE.ToString()))
                                {
                                    //获取最早上市时间
                                    var his = await m_historicalService.RequestEarliestDataPointAsync(symbol, exchange);
                                    var earliestDate = new DateTime();
                                    if (!string.IsNullOrEmpty(his))
                                    {
                                        his = his.Insert(4, "-").Insert(7, "-");
                                        earliestDate = DateTime.Parse(his);
                                    }

                                    data = new Stock
                                    {
                                        Name = name,
                                        Symbol = detail.Contract.Symbol,
                                        ConId = detail.Contract.ConId,
                                        Currency = detail.Contract.Currency,
                                        Exchange = exchange,
                                        EarliestDate = earliestDate,
                                        Category = category,
                                    };
                                    m_repoStock.Add(data);
                                    if (saveToDb)
                                        await m_repoStock.SaveChangesAsync();

                                    m_logger.LogWarning("Stock, Saved={0}, earliestDate={1}", symbol, earliestDate);
                                    break;
                                }
                            }
                            catch (Exception e)
                            {
                                m_logger.LogError("UpdateStockToDb Error: {0}: {1}\n{2}", symbol, e.Message, e.StackTrace);
                            }
                        }
                    }
                }
                else
                    m_logger.LogError("{0}:{1} Not Found!!!", name, symbol);
            }
            return data;
        }

        /// <summary>
        /// 搜索股票代码
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        [Api]
        public async Task<ContractDescription[]> SearchSymbolsAsync(string pattern)
        {
            int reqId = MoatCommon.GetReqId(pattern);
            ibClient.ClientSocket.reqMatchingSymbols(reqId, pattern);
            return await SendRequestAsync(reqId);
        }

        private void HandleSymbolSamplesData(SymbolSamplesMessage message)
        {
            m_results[message.ReqId] = message.ContractDescriptions;
            HandleResponse(message.ReqId);

            //var datalist = new List<ContractDescription>();
            //string symbol = string.Empty;
            //if (Common.CheckValidReqId(message.ReqId, out symbol))
            //{
            //    foreach (ContractDescription contractDescription in message.ContractDescriptions)
            //    {
            //        datalist.Add(contractDescription);

            //        var derivSecTypes = "";
            //        foreach (var derivSecType in contractDescription.DerivativeSecTypes)
            //        {
            //            derivSecTypes += derivSecType;
            //            derivSecTypes += " ";
            //        }

            //        m_logger.LogInformation("Contract: conId - {0}, symbol - {1}, secType - {2}, primExchange - {3}, currency - {4}, derivativeSecTypes - {5}",
            //             contractDescription.Contract.ConId, contractDescription.Contract.Symbol, contractDescription.Contract.SecType,
            //             contractDescription.Contract.PrimaryExch, contractDescription.Contract.Currency, derivSecTypes);
            //    }
            //}
        }
    }
}
