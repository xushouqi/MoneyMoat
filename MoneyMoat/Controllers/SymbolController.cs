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
    public class SymbolController : Controller
    {
        private readonly MoneyMoat.Services.SymbolService _actionService;

        public SymbolController(MoneyMoat.Services.SymbolService actionService)
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
        public async Task<IActionResult> FindAsync(string symbol, string sign)
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
					var retData = await _actionService.FindAsync(tmp["symbol"]);
					var data = Mapper.Map<StockData>(retData);

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
				var retData = await _actionService.FindAsync(symbol);
				var data = Mapper.Map<StockData>(retData);

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAsync(string sign)
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
				if (tmp != null)
				{
					var retData = await _actionService.GetAllAsync();
					var data = new List<StockData>();
                    for (int i = 0; i < retData.Count; i++)
                        data.Add(Mapper.Map<StockData>(retData[i]));

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
				var retData = await _actionService.GetAllAsync();
				var data = new List<StockData>();
                    for (int i = 0; i < retData.Count; i++)
                        data.Add(Mapper.Map<StockData>(retData[i]));

				if (data != null)
				{
					return new OkObjectResult(data);
				}
				else
					return NoContent();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStock(string name, string symbol, string category, bool saveToDb, string sign)
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
				if (tmp != null && tmp.ContainsKey("name") && tmp.ContainsKey("symbol") && tmp.ContainsKey("category") && tmp.ContainsKey("saveToDb"))
				{
					var retData = await _actionService.UpdateStock(tmp["name"], tmp["symbol"], tmp["category"], bool.Parse(tmp["saveToDb"]));
					var data = Mapper.Map<StockData>(retData);

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
				var retData = await _actionService.UpdateStock(name, symbol, category, saveToDb);
				var data = Mapper.Map<StockData>(retData);

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
