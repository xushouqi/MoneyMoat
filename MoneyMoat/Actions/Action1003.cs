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
	
    public class Action1003 : ActionBase<int>
    {
        private readonly AnalyserService m_service;

        public Action1003(
            AnalyserService service)
        {
            m_service = service;
            m_actionId = 1003;
        }
        
        public override async Task DoAction()
        {
            if (m_params != null)
            {

                var retData = await m_service.UpdateAllHistoricals();
				var data = retData;

                m_return = data;
            }
            await base.DoAction();
        }
    }
}
