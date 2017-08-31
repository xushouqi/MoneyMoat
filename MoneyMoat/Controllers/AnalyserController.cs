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

namespace MoneyMoat.Controllers
{
    [EnableCors("AllowSameDomain")]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AnalyserController : Controller
    {
        private readonly MoneyMoat.Services.AnalyserService _actionService;

        public AnalyserController(MoneyMoat.Services.AnalyserService actionService)
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
        public async Task<IActionResult> UpdateAllFundamentals(int interval, int count, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("interval") && tmp.ContainsKey("count"))
				{
					var retData = await _actionService.UpdateAllFundamentals(int.Parse(tmp["interval"]), int.Parse(tmp["count"]));
					if (retData != null)
					{
						return new OkObjectResult(retData);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateAllFundamentals(interval, count);
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAllHistoricals(int interval, int count, string sign)
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
                design = RsaService.DecryptToString(datas, 0);
            }
            if (!string.IsNullOrEmpty(design))
            {
				var tmp = Common.QueryStringToData(design);
				if (tmp != null && tmp.ContainsKey("interval") && tmp.ContainsKey("count"))
				{
					var retData = await _actionService.UpdateAllHistoricals(int.Parse(tmp["interval"]), int.Parse(tmp["count"]));
					if (retData != null)
					{
						return new OkObjectResult(retData);
					}
					else
						return NoContent();
				}
				else
					return new UnauthorizedResult();
            }
            else
            {
				var retData = await _actionService.UpdateAllHistoricals(interval, count);
				if (retData != null)
				{
					return new OkObjectResult(retData);
				}
				else
					return NoContent();
            }
        }

		
    }
}
