using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyMoat.Services;
using CommonLibs;
using CommonNetwork;
using AutoMapper;
using StockModels;
using StockModels.ViewModels;

namespace MoneyMoat.Actions
{
	
    public class Action1005 : ActionBase<int>
    {
        private readonly AnalyserService m_service;

        public Action1005(
            AnalyserService service)
        {
            m_service = service;
            m_actionId = 1005;
        }
        
        public override async Task DoAction()
        {
            if (m_params != null)
            {
                var symbol = m_params.ReadString();

                var retData = await m_service.UpdateAndCalcFundamental(symbol);
				var data = retData;

                m_return = data;
            }
            await base.DoAction();
        }
    }
}
