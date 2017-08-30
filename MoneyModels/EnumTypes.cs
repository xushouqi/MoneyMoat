
namespace MoneyModels
{
    public enum CurrencyEnum
    {
        USD,
        HKD,
        CNY,
        EUR
    }
    public enum ExchangeEnum
    {
        ISLAND,
        NYSE,
        SEHK,
        SHSE,
        SZSE,
    }
    public enum SecTypeEnum
    {
        STK,
        OPT,
        FUT,
        CASH,
        FUND,
        IND,
        NEWS,
        CFD,
    }
    public enum StockTypeFilterEnum
    {
        ALL,
        //Corporation
        CORP,
        //American Depositary Receipt
        ADR,
        //Exchange Traded Fund
        ETF,
        //Real Estate Investment Trust
        REIT,
        //Closed End Fund
        CEF,
    }
    public enum PeriodTypeEnum
    {
        Annual = 0,
        Quarter,
        TTM,
    }
    public enum FYTypeEnum
    {
        //Net Asset Value, 净资产价值法，目前地产行业的主流估值方法
        NAV,
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
        ReportsOwnership,
        ReportSnapshot,
        ReportsFinSummary,
        ReportsFinStatements,
        RESC,
        //CalendarReport
    }

    public enum ScanCodeEnum
    {
        LOW_OPT_VOL_PUT_CALL_RATIO,
        HIGH_OPT_IMP_VOLAT_OVER_HIST,
        LOW_OPT_IMP_VOLAT_OVER_HIST,
        HIGH_OPT_IMP_VOLAT,
        TOP_OPT_IMP_VOLAT_GAIN,
        TOP_OPT_IMP_VOLAT_LOSE,
        HIGH_OPT_VOLUME_PUT_CALL_RATIO,
        LOW_OPT_VOLUME_PUT_CALL_RATIO,
        OPT_VOLUME_MOST_ACTIVE,
        HOT_BY_OPT_VOLUME,
        HIGH_OPT_OPEN_INTEREST_PUT_CALL_RATIO,
        LOW_OPT_OPEN_INTEREST_PUT_CALL_RATIO,
        TOP_PERC_GAIN,
        MOST_ACTIVE,
        TOP_PERC_LOSE,
        HOT_BY_VOLUME,
        HOT_BY_PRICE,
        TOP_TRADE_COUNT,
        TOP_TRADE_RATE,
        TOP_PRICE_RANGE,
        HOT_BY_PRICE_RANGE,
        TOP_VOLUME_RATE,
        LOW_OPT_IMP_VOLAT,
        OPT_OPEN_INTEREST_MOST_ACTIVE,
        NOT_OPEN,
        HALTED,
        TOP_OPEN_PERC_GAIN,
        TOP_OPEN_PERC_LOSE,
        HIGH_OPEN_GAP,
        LOW_OPEN_GAP,
        HIGH_VS_13W_HL,
        LOW_VS_13W_HL,
        HIGH_VS_26W_HL,
        LOW_VS_26W_HL,
        HIGH_VS_52W_HL,
        LOW_VS_52W_HL,
        HIGH_SYNTH_BID_REV_NAT_YIELD,
        LOW_SYNTH_BID_REV_NAT_YIELD
    }


}
