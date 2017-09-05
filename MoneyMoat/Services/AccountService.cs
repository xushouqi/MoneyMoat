using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using MoneyMoat.Types;
using StockModels;
using IBApi;
using YAXLib;
using CommonLibs;

namespace MoneyMoat.Services
{
    [WebApi]
    public class AccountService : IBServiceBase<string>
    {
        private const int ACCOUNT_ID_BASE = 50000000;
        private const int ACCOUNT_SUMMARY_ID = ACCOUNT_ID_BASE + 1;

        private const string ACCOUNT_SUMMARY_TAGS = "AccountType,NetLiquidation,TotalCashValue,SettledCash,AccruedCash,BuyingPower,EquityWithLoanValue,PreviousEquityWithLoanValue,"
             + "GrossPositionValue,ReqTEquity,ReqTMargin,SMA,InitMarginReq,MaintMarginReq,AvailableFunds,ExcessLiquidity,Cushion,FullInitMarginReq,FullMaintMarginReq,FullAvailableFunds,"
             + "FullExcessLiquidity,LookAheadNextChange,LookAheadInitMarginReq ,LookAheadMaintMarginReq,LookAheadAvailableFunds,LookAheadExcessLiquidity,HighestSeverity,DayTradesRemaining,Leverage";
        
        private int activeReqId = 0;

        public AccountService(IBManager ibmanager,
                        CommonManager commonManager,
                        ILogger<IBManager> logger) : base(ibmanager, logger, commonManager)
        {
            ibClient.AccountSummary += HandleAccountSummary;
            ibClient.AccountSummaryEnd += reqId => { m_logger.LogWarning("AccountSummaryEnd. " + reqId + "\r\n"); activeReqId = 0; };
        }

        public void RequestAccountSummery()
        {
            if (activeReqId == 0)
            {
                activeReqId = ACCOUNT_SUMMARY_ID;
                m_clientSocket.reqAccountSummary(activeReqId, "All", ACCOUNT_SUMMARY_TAGS);
            }
        }
        public void CancelAccountSummary()
        {
            if (activeReqId > 0)
            {
                m_clientSocket.cancelAccountSummary(activeReqId);
                activeReqId = 0;
            }
        }
        private void HandleAccountSummary(AccountSummaryMessage summaryMessage)
        {
            m_logger.LogWarning("HandleAccountSummary: Account={0}, Value={1}, Currency={2}, Tag={3}",
            summaryMessage.Account, summaryMessage.Value, summaryMessage.Currency, summaryMessage.Tag);
        }

    }
}
