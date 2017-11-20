﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibs;


namespace StockModels
{
    [DataModels]
    public class XueQiuQuote : Entity<int>
    {
        public string Symbol { get; set; }
        
        public float volume { get; set; } = 0f;
        public float lot_volume { get; set; } = 0f;
        public float volumeAverage { get; set; } = 0f;
        public float marketCapital { get; set; } = 0f;

        public float current { get; set; } = 0f;
        public float percentage { get; set; } = 0f;
        public float change { get; set; } = 0f;
        public float last_close { get; set; } = 0f;
        public float high52week { get; set; } = 0f;
        public float low52week { get; set; } = 0f;

        public float eps { get; set; } = 0f;
        public float pe_ttm { get; set; } = 0f;
        public string pe_lyr { get; set; }
        public float beta { get; set; } = 0f;

        public int totalShares { get; set; } = 0;
        public DateTime time { get; set; }
        public float afterHours { get; set; } = 0f;
        public float afterHoursPct { get; set; } = 0f;
        public float afterHoursChg { get; set; } = 0f;
        public DateTime afterHoursTime { get; set; } = DateTime.MinValue;

        public long updateAt { get; set; } = 0;
        public float dividend { get; set; } = 0f;
        public float yield { get; set; } = 0f;
        public string turnover_rate { get; set; }
        public float instOwn { get; set; } = 0f;
        public float rise_stop { get; set; } = 0f;
        public float fall_stop { get; set; } = 0f;

        public float amount { get; set; } = 0f;
        public float net_assets { get; set; } = 0f;
        public string amplitude { get; set; }
        public float pb { get; set; } = 0f;
        public float benefit_before_tax { get; set; } = 0f;
        public float benefit_after_tax { get; set; } = 0f;
        public string convert_bond_ratio { get; set; }
        public string totalissuescale { get; set; }
        public string outstandingamt { get; set; }

        //public float circulation { get; set; } = 0f;
        //public float par_value { get; set; } = 0f;
        //public int after_hour_vol { get; set; } = 0;
        //public float float_market_capital { get; set; } = 0f;
        //public float volume_ratio { get; set; } = 0f;
        //public float percent5m { get; set; } = 0f;
        //public DateTime ex_dividend_date { get; set; } = DateTime.MinValue;
        //public float moving_avg_200_day { get; set; } = 0f;
        //public float chg_from_200_day_moving_avg { get; set; } = 0f;
        //public float pct_chg_from_200_day_moving_avg { get; set; } = 0f;
        //public float moving_avg_50_day { get; set; } = 0f;
        //public float chg_from_50_day_moving_avg { get; set; } = 0f;
        //public float pct_chg_from_50_day_moving_avg { get; set; } = 0f;
        //public float ebitda { get; set; } = 0f;
        //public float short_ratio { get; set; } = 0f;
        //public float peg_ratio { get; set; } = 0f;
        //public float pe_estimate_next_year { get; set; } = 0f;
        //public float eps_estimate_next_year { get; set; } = 0f;
        //public float eps_estimate_next_quarter { get; set; } = 0f;
        //public float eps_estimate_current_quarter { get; set; } = 0f;
        //public float psr { get; set; } = 0f;
        //public float revenue { get; set; } = 0f;
        //public float profit_margin { get; set; } = 0f;
    }
}
