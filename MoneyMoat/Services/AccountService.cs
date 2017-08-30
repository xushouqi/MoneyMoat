using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MoneyMoat.Messages;
using MoneyMoat.Types;
using MoneyModels;
using IBApi;
using YAXLib;

namespace MoneyMoat.Services
{
     class AccountService
    {
        private const int ACCOUNT_ID_BASE = 50000000;
        private const int ACCOUNT_SUMMARY_ID = ACCOUNT_ID_BASE + 1;

        private const string ACCOUNT_SUMMARY_TAGS = "AccountType,NetLiquidation,TotalCashValue,SettledCash,AccruedCash,BuyingPower,EquityWithLoanValue,PreviousEquityWithLoanValue,"
             + "GrossPositionValue,ReqTEquity,ReqTMargin,SMA,InitMarginReq,MaintMarginReq,AvailableFunds,ExcessLiquidity,Cushion,FullInitMarginReq,FullMaintMarginReq,FullAvailableFunds,"
             + "FullExcessLiquidity,LookAheadNextChange,LookAheadInitMarginReq ,LookAheadMaintMarginReq,LookAheadAvailableFunds,LookAheadExcessLiquidity,HighestSeverity,DayTradesRemaining,Leverage";

        private readonly ILogger m_logger;
        private readonly IBClient ibClient;
        private int activeReqId = 0;

        public AccountService(IBClient ibclient,
                        ILogger<IBManager> logger)
        {
            m_logger = logger;
            ibClient = ibclient;

            ibClient.AccountSummary += HandleAccountSummary;
            ibClient.AccountSummaryEnd += reqId => { m_logger.LogInformation("AccountSummaryEnd. " + reqId + "\r\n"); activeReqId = 0; };
        }

        public void RequestAccountSummery()
        {
            if (activeReqId == 0)
            {
                activeReqId = ACCOUNT_SUMMARY_ID;
                ibClient.ClientSocket.reqAccountSummary(activeReqId, "All", ACCOUNT_SUMMARY_TAGS);
            }
        }
        public void CancelAccountSummary()
        {
            if (activeReqId > 0)
            {
                ibClient.ClientSocket.cancelAccountSummary(activeReqId);
                activeReqId = 0;
            }
        }
        private void HandleAccountSummary(AccountSummaryMessage summaryMessage)
        {
            m_logger.LogInformation("HandleAccountSummary: Account={0}, Value={1}, Currency={2}, Tag={3}",
            summaryMessage.Account, summaryMessage.Value, summaryMessage.Currency, summaryMessage.Tag);
        }

    }
}
