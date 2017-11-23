using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommonLibs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        private readonly ILogger _logger;

        public HistoricalController(ILoggerFactory logFactory, 
            MoneyMoat.Services.HistoricalService actionService)
        {
            _actionService = actionService;
            _logger = logFactory.CreateLogger("Error");
        }
		
        private int GetCurrentAccountId()
        {
            int accountid = -1;
            int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out accountid);
            return accountid;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateHistoricalDataFromXueQiu(string symbol, string sign)
        {
            try
            {
				string design = string.Empty;
				if (!string.IsNullOrEmpty(sign))
				{
					design = RsaService.DecryptToString(sign, "");
				}
				else if (Request.ContentLength != null)
				{
					byte[] datas = new byte[(int)Request.ContentLength];
					var ret = Request.Body.Read(datas, 0, (int)Request.ContentLength);
					design = RsaService.DecryptToString(datas, "");
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
            catch(Exception e)
            {
                _logger.LogError(string.Format("Exception={0}\n ExceptionSource={1}\n StackTrace={2}",
                    e.Message, e.Source, e.StackTrace));
                return new BadRequestResult();
            }
        }

		
    }
}
