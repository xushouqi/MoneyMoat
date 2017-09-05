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
    public class AccountController : Controller
    {
        private readonly MoneyMoat.Services.AccountService _actionService;

        public AccountController(MoneyMoat.Services.AccountService actionService)
        {
            _actionService = actionService;
        }
		
        private int GetCurrentAccountId()
        {
            int accountid = int.Parse(User.Claims.First().Value);
            return accountid;
        }


		
    }
}
