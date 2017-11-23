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
    public class Action1001 : ActionBase<int>
    {
        private readonly AnalyserService m_service;

        public Action1001(
            AnalyserService service)
        {
            m_service = service;
            m_actionId = 1001;
        }
        
        public override async Task DoAction()
        {
            if (m_params != null)
            {
                var delay = m_params.ReadInt();

                var retData = await m_service.StopAllTasks(delay);
				var data = retData;

                m_return = data;
            }
            await base.DoAction();
        }
    }
}
