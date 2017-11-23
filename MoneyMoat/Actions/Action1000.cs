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
	[AuthPolicy(AuthPolicy = UserTypeEnum.None)]
    public class Action1000 : ActionBase<int>
    {
        private readonly AnalyserService m_service;

        public Action1000(
            AnalyserService service)
        {
            m_service = service;
            m_actionId = 1000;
        }
        
        public override async Task DoAction()
        {
            if (m_params != null)
            {
                var saveToDb = m_params.ReadBool();

                var retData = await m_service.UpdateStockSymbolsFromSina(saveToDb);
				var data = new ReturnData<int>(retData);

                m_return = data;
            }
            await base.DoAction();
        }
    }
}
