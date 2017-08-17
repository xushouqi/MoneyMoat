using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyMoat.Types
{
    public enum ExchangeEnum
    {
        ISLAND,
        NYSE,
    }

    public enum FYActualEnum
    {
        //资本支出，为了获得固定资产，或为了延长固定资产耐用年限而流出的费用        
        CAPEX,
        //盈利
        PPROFIT,
        //净利润
        NPROFIT,
        //???
        NPROFITREP,
        //???
        PPROFITREP,
        //资产收益率 =（利润总额+利息收入）/ 总资产总额Assets
        ROA,
        //股本回报率＝净收入／股东股本（shareholder's equity）
        ROE,
        //cash flow per share?
        CPS,
        //Dividend per share, DPS = 当期发放的现金股利总额 ÷ 总股本
        DPS,
        // Earnings Per Share
        EPS,
        //???
        EPSREP,
        //收入
        REVENUE,
        // earnings before interest and taxes = revenue – operating expenses (OPEX)
        EBIT,
        //税息折旧及摊销前利润，Earnings Before Interest, Taxes, Depreciation折旧 and Amortization摊销
        EBITDA,
        //Book Value Per Share 每股账面净值 = （资产-负债）/已发行的股数
        BVPS,
        //???
        NDEBT,
    }
    public enum WhatToShowEnum
    {
        TRADES,
        MIDPOINT,
        BID,
        ASK,
        BID_ASK,
        HISTORICAL_VOLATILITY,
        OPTION_IMPLIED_VOLATILITY,
        YIELD_BID,
        YIELD_ASK,
        YIELD_BID_ASK,
        YIELD_LAST,
        ADJUSTED_LAST
    }

    public enum FundamentalsReportEnum
    {
        ReportSnapshot,
        ReportsFinSummary,
        ReportsFinStatements,
        RESC
    }
}
