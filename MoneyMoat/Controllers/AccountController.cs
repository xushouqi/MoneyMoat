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
    public class AccountController : Controller
    {
        private readonly MoneyMoat.Services.AccountService _actionService;
        private readonly ILogger _logger;

        public AccountController(ILoggerFactory logFactory, 
            MoneyMoat.Services.AccountService actionService)
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


		
    }
}
