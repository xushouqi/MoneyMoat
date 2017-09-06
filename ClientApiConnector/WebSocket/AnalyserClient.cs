using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommonLibs;
using StockModels.ViewModels;

namespace CommonNetwork.MoneyMoat
{
	public class AnalyserClient
	{
		private ClientSocket m_client;
		public AnalyserClient(ClientSocket client)
		{
			m_client = client;
			
            client.RegActions[1001] = OnStopAllTasksCallBack;
            client.RegActions[1002] = OnUpdateAllFundamentalsCallBack;
            client.RegActions[1003] = OnUpdateAllHistoricalsCallBack;
            client.RegActions[1004] = OnCalcFinSummaryCallBack;
            client.RegActions[1005] = OnUpdateAndCalcFundamentalCallBack;

		}

		    
		public void SubmitStopAllTasks(int delay, Action<ErrorCodeEnum, int> callback)
		{
			if (m_client.CheckConnection() && m_StopAllTasksCallback == null)
			{
				var pars = new PackageParams();
			    pars.WriteByte(delay);

				var pardata = pars.PopBuffer();
				pars.Dispose();

				var package = m_client.Send(1001, pardata, OnStopAllTasksCallBack);
				if (package != null && callback != null)
					m_StopAllTasksCallback = callback;
			}
		}
	
    
		public async Task<ReturnData<int>> SubmitStopAllTasksAsync(int delay)
		{
			ReturnData<int> retData = null;
            if (m_client.CheckConnection())
            {
                var pars = new PackageParams();
			    pars.WriteByte(delay);

                var pardata = pars.PopBuffer();
                pars.Dispose();

                var package = await m_client.SendAsync(1001, pardata);
                retData = new ReturnData<int>((ErrorCodeEnum)package.ErrorCode);
				if (package.MyError == ErrorCodeEnum.Success)
					retData.Data = ProtoBufUtils.Deserialize<int>(package.Return);              
            }
            return retData;
		}
	
    
		public void SubmitUpdateAllFundamentals(bool forceUpdate, Action<ErrorCodeEnum, int> callback)
		{
			if (m_client.CheckConnection() && m_UpdateAllFundamentalsCallback == null)
			{
				var pars = new PackageParams();
			    pars.WriteByte(forceUpdate);

				var pardata = pars.PopBuffer();
				pars.Dispose();

				var package = m_client.Send(1002, pardata, OnUpdateAllFundamentalsCallBack);
				if (package != null && callback != null)
					m_UpdateAllFundamentalsCallback = callback;
			}
		}
	
    
		public async Task<ReturnData<int>> SubmitUpdateAllFundamentalsAsync(bool forceUpdate)
		{
			ReturnData<int> retData = null;
            if (m_client.CheckConnection())
            {
                var pars = new PackageParams();
			    pars.WriteByte(forceUpdate);

                var pardata = pars.PopBuffer();
                pars.Dispose();

                var package = await m_client.SendAsync(1002, pardata);
                retData = new ReturnData<int>((ErrorCodeEnum)package.ErrorCode);
				if (package.MyError == ErrorCodeEnum.Success)
					retData.Data = ProtoBufUtils.Deserialize<int>(package.Return);              
            }
            return retData;
		}
	
    
		public void SubmitUpdateAllHistoricals(Action<ErrorCodeEnum, int> callback)
		{
			if (m_client.CheckConnection() && m_UpdateAllHistoricalsCallback == null)
			{
				var pars = new PackageParams();

				var pardata = pars.PopBuffer();
				pars.Dispose();

				var package = m_client.Send(1003, pardata, OnUpdateAllHistoricalsCallBack);
				if (package != null && callback != null)
					m_UpdateAllHistoricalsCallback = callback;
			}
		}
	
    
		public async Task<ReturnData<int>> SubmitUpdateAllHistoricalsAsync()
		{
			ReturnData<int> retData = null;
            if (m_client.CheckConnection())
            {
                var pars = new PackageParams();

                var pardata = pars.PopBuffer();
                pars.Dispose();

                var package = await m_client.SendAsync(1003, pardata);
                retData = new ReturnData<int>((ErrorCodeEnum)package.ErrorCode);
				if (package.MyError == ErrorCodeEnum.Success)
					retData.Data = ProtoBufUtils.Deserialize<int>(package.Return);              
            }
            return retData;
		}
	
    
		public void SubmitCalcFinSummary(string symbol, Action<ErrorCodeEnum, List<FinSummaryData>> callback)
		{
			if (m_client.CheckConnection() && m_CalcFinSummaryCallback == null)
			{
				var pars = new PackageParams();
			    pars.WriteByte(symbol);

				var pardata = pars.PopBuffer();
				pars.Dispose();

				var package = m_client.Send(1004, pardata, OnCalcFinSummaryCallBack);
				if (package != null && callback != null)
					m_CalcFinSummaryCallback = callback;
			}
		}
	
    
		public async Task<ReturnData<List<FinSummaryData>>> SubmitCalcFinSummaryAsync(string symbol)
		{
			ReturnData<List<FinSummaryData>> retData = null;
            if (m_client.CheckConnection())
            {
                var pars = new PackageParams();
			    pars.WriteByte(symbol);

                var pardata = pars.PopBuffer();
                pars.Dispose();

                var package = await m_client.SendAsync(1004, pardata);
                retData = new ReturnData<List<FinSummaryData>>((ErrorCodeEnum)package.ErrorCode);
				if (package.MyError == ErrorCodeEnum.Success)
					retData.Data = ProtoBufUtils.Deserialize<List<FinSummaryData>>(package.Return);              
            }
            return retData;
		}
	
    
		public void SubmitUpdateAndCalcFundamental(string symbol, Action<ErrorCodeEnum, int> callback)
		{
			if (m_client.CheckConnection() && m_UpdateAndCalcFundamentalCallback == null)
			{
				var pars = new PackageParams();
			    pars.WriteByte(symbol);

				var pardata = pars.PopBuffer();
				pars.Dispose();

				var package = m_client.Send(1005, pardata, OnUpdateAndCalcFundamentalCallBack);
				if (package != null && callback != null)
					m_UpdateAndCalcFundamentalCallback = callback;
			}
		}
	
    
		public async Task<ReturnData<int>> SubmitUpdateAndCalcFundamentalAsync(string symbol)
		{
			ReturnData<int> retData = null;
            if (m_client.CheckConnection())
            {
                var pars = new PackageParams();
			    pars.WriteByte(symbol);

                var pardata = pars.PopBuffer();
                pars.Dispose();

                var package = await m_client.SendAsync(1005, pardata);
                retData = new ReturnData<int>((ErrorCodeEnum)package.ErrorCode);
				if (package.MyError == ErrorCodeEnum.Success)
					retData.Data = ProtoBufUtils.Deserialize<int>(package.Return);              
            }
            return retData;
		}
	

	
		    
		private Action<ErrorCodeEnum, int> m_StopAllTasksCallback = null;
		private Action<ErrorCodeEnum, int> m_StopAllTasksCallbackAdd = null;

		void OnStopAllTasksCallBack(WebPackage package)
		{
			if (m_StopAllTasksCallback != null || m_StopAllTasksCallbackAdd != null)
			{
				var retData = ProtoBufUtils.Deserialize<int>(package.Return);

				if (m_StopAllTasksCallback != null)
				{
					m_StopAllTasksCallback(package.MyError, retData);
					m_StopAllTasksCallback = null;
				}
			
				if (m_StopAllTasksCallbackAdd != null)
					m_StopAllTasksCallbackAdd(package.MyError, retData);
			}
		}
    
		private Action<ErrorCodeEnum, int> m_UpdateAllFundamentalsCallback = null;
		private Action<ErrorCodeEnum, int> m_UpdateAllFundamentalsCallbackAdd = null;

		void OnUpdateAllFundamentalsCallBack(WebPackage package)
		{
			if (m_UpdateAllFundamentalsCallback != null || m_UpdateAllFundamentalsCallbackAdd != null)
			{
				var retData = ProtoBufUtils.Deserialize<int>(package.Return);

				if (m_UpdateAllFundamentalsCallback != null)
				{
					m_UpdateAllFundamentalsCallback(package.MyError, retData);
					m_UpdateAllFundamentalsCallback = null;
				}
			
				if (m_UpdateAllFundamentalsCallbackAdd != null)
					m_UpdateAllFundamentalsCallbackAdd(package.MyError, retData);
			}
		}
    
		private Action<ErrorCodeEnum, int> m_UpdateAllHistoricalsCallback = null;
		private Action<ErrorCodeEnum, int> m_UpdateAllHistoricalsCallbackAdd = null;

		void OnUpdateAllHistoricalsCallBack(WebPackage package)
		{
			if (m_UpdateAllHistoricalsCallback != null || m_UpdateAllHistoricalsCallbackAdd != null)
			{
				var retData = ProtoBufUtils.Deserialize<int>(package.Return);

				if (m_UpdateAllHistoricalsCallback != null)
				{
					m_UpdateAllHistoricalsCallback(package.MyError, retData);
					m_UpdateAllHistoricalsCallback = null;
				}
			
				if (m_UpdateAllHistoricalsCallbackAdd != null)
					m_UpdateAllHistoricalsCallbackAdd(package.MyError, retData);
			}
		}
    
		private Action<ErrorCodeEnum, List<FinSummaryData>> m_CalcFinSummaryCallback = null;
		private Action<ErrorCodeEnum, List<FinSummaryData>> m_CalcFinSummaryCallbackAdd = null;

		void OnCalcFinSummaryCallBack(WebPackage package)
		{
			if (m_CalcFinSummaryCallback != null || m_CalcFinSummaryCallbackAdd != null)
			{
				var retData = ProtoBufUtils.Deserialize<List<FinSummaryData>>(package.Return);

				if (m_CalcFinSummaryCallback != null)
				{
					m_CalcFinSummaryCallback(package.MyError, retData);
					m_CalcFinSummaryCallback = null;
				}
			
				if (m_CalcFinSummaryCallbackAdd != null)
					m_CalcFinSummaryCallbackAdd(package.MyError, retData);
			}
		}
    
		private Action<ErrorCodeEnum, int> m_UpdateAndCalcFundamentalCallback = null;
		private Action<ErrorCodeEnum, int> m_UpdateAndCalcFundamentalCallbackAdd = null;

		void OnUpdateAndCalcFundamentalCallBack(WebPackage package)
		{
			if (m_UpdateAndCalcFundamentalCallback != null || m_UpdateAndCalcFundamentalCallbackAdd != null)
			{
				var retData = ProtoBufUtils.Deserialize<int>(package.Return);

				if (m_UpdateAndCalcFundamentalCallback != null)
				{
					m_UpdateAndCalcFundamentalCallback(package.MyError, retData);
					m_UpdateAndCalcFundamentalCallback = null;
				}
			
				if (m_UpdateAndCalcFundamentalCallbackAdd != null)
					m_UpdateAndCalcFundamentalCallbackAdd(package.MyError, retData);
			}
		}

		

		public void AddToStopAllTasks(Action<ErrorCodeEnum, int> callback)
		{
			m_StopAllTasksCallbackAdd += callback;
		}
		public void RemoveStopAllTasks(Action<ErrorCodeEnum, int> callback)
		{
			m_StopAllTasksCallbackAdd -= callback;
		}

		public void AddToUpdateAllFundamentals(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAllFundamentalsCallbackAdd += callback;
		}
		public void RemoveUpdateAllFundamentals(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAllFundamentalsCallbackAdd -= callback;
		}

		public void AddToUpdateAllHistoricals(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAllHistoricalsCallbackAdd += callback;
		}
		public void RemoveUpdateAllHistoricals(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAllHistoricalsCallbackAdd -= callback;
		}

		public void AddToCalcFinSummary(Action<ErrorCodeEnum, List<FinSummaryData>> callback)
		{
			m_CalcFinSummaryCallbackAdd += callback;
		}
		public void RemoveCalcFinSummary(Action<ErrorCodeEnum, List<FinSummaryData>> callback)
		{
			m_CalcFinSummaryCallbackAdd -= callback;
		}

		public void AddToUpdateAndCalcFundamental(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAndCalcFundamentalCallbackAdd += callback;
		}
		public void RemoveUpdateAndCalcFundamental(Action<ErrorCodeEnum, int> callback)
		{
			m_UpdateAndCalcFundamentalCallbackAdd -= callback;
		}


	}
}