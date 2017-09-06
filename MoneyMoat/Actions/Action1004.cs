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
	
    public class Action1004 : ActionBase<List<FinSummaryData>>
    {
        private readonly AnalyserService m_service;

        public Action1004(
            AnalyserService service)
        {
            m_service = service;
            m_actionId = 1004;
        }
        
        public override async Task DoAction()
        {
            if (m_params != null)
            {
                var symbol = m_params.ReadString();

                var retData = await m_service.CalcFinSummary(symbol);
				var dataList = new List<FinSummaryData>();
                    for (int i = 0; i < retData.Data.Count; i++)
                        dataList.Add(Mapper.Map<FinSummaryData>(retData.Data[i]));
                    var data = new ReturnData<List<FinSummaryData>>{
                        ErrorCode = retData.ErrorCode,
                        Data = dataList,
                    };

                m_return = data;
            }
            await base.DoAction();
        }
    }
}
