using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyMoat.Services;
using MoneyModels;

namespace MoneyMoat.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AnalyserController : Controller
    {
        private readonly IRepository<Stock> m_repoStock;
        private readonly IRepository<Financal> m_repoFin;
        private readonly IRepository<FYEstimate> m_repoEst;
        private readonly IRepository<NPEstimate> m_repoNPE;
        private readonly IRepository<XueQiuData> m_repoDatas;
        private readonly IRepository<FinSummary> m_repoSummary;
        private readonly IRepository<FinStatement> m_repoStatement;

        public AnalyserController(
                        IRepository<Stock> repoStock,
                        IRepository<Financal> repoFin,
                        IRepository<FYEstimate> repoEst,
                        IRepository<NPEstimate> repoNPE,
                        IRepository<XueQiuData> repoDatas,
                        IRepository<FinSummary> repoSummary,
                        IRepository<FinStatement> repoStatement)
        {
            m_repoStock = repoStock;
            m_repoFin = repoFin;
            m_repoEst = repoEst;
            m_repoNPE = repoNPE;
            m_repoDatas = repoDatas;
            m_repoSummary = repoSummary;
            m_repoStatement = repoStatement;
        }

        [HttpPost]
        public async Task<IActionResult> GetHistoricalDatas(string symbol, DateTime start, DateTime end)
        {
            var retData = await m_repoDatas.WhereToListAsync(t => t.Symbol == symbol && t.time >= start && t.time < end.AddDays(1),
                                                        t => t.time);
            if (retData != null)
            {
                return new OkObjectResult(retData);
            }
            else
                return NoContent();
        }

    }
}