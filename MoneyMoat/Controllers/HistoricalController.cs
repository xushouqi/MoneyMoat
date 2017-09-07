using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using AutoMapper;
using Microsoft.Extensions.Logging;
using StockModels;
using StockModels.ViewModels;

namespace MoneyMoat.Controllers
{
    [EnableCors("AllowSameDomain")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class HistoricalController : Controller
    {
        private readonly MoneyMoat.Services.HistoricalService _actionService;

        public HistoricalController(MoneyMoat.Services.HistoricalService actionService)
        {
            _actionService = actionService;
        }
		
        private int GetCurrentAccountId()
        {
            int accountid = int.Parse(User.Claims.First().Value);
            return accountid;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateHistoricalDataFromXueQiu(string symbol, string sign)
        {
			string design = string.Empty;
            if (!string.IsNullOrEmpty(sign))
            {
                design = RsaService.DecryptToString(sign);
            }
            else if (Request.ContentLength != null)
            {
                byte[] datas = new byte[(int)Request.ContentLength];
                var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
                design = RsaService.DecryptToString(datas);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("symbol"))
				{
					var retData = await _actionService.UpdateHistoricalDataFromXueQiu(tmp["symbol"]);
					var dataValue = Mapper.Map<HistoricalData>(retData);
var data = new ReturnData<HistoricalData>(dataValue);

					if (data != null)
					{
						return new OkObjectResult(data);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateHistoricalDataFromXueQiu(symbol);
				var dataValue = Mapper.Map<HistoricalData>(retData);
var data = new ReturnData<HistoricalData>(dataValue);

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

		
    }
}
