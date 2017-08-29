using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MoneyModels
{
    public class XueQiuQuote : Entity
    {
        public override int GetId()
        {
            return Id;
        }
        public override string GetKey()
        {
            throw new NotImplementedException();
        }
        public override DateTime TryUpdateTime()
        {
            return UpdateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public string Symbol { get; set; }


        public float volume { get; set; }
        public float lot_volume { get; set; }
        public float volumeAverage { get; set; }
        public float marketCapital { get; set; }

        public float current { get; set; } = 0f;
        public float percentage { get; set; } = 0f;
        public float change { get; set; } = 0f;
        public float last_close { get; set; } = 0f;
        public float high52week { get; set; } = 0f;
        public float low52week { get; set; } = 0f;

        public float eps { get; set; } = 0f;
        public float pe_ttm { get; set; } = 0f;
        public float pe_lyr { get; set; } = 0f;
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
        public float turnover_rate { get; set; } = 0f;
        public float instOwn { get; set; } = 0f;
        public float rise_stop { get; set; } = 0f;
        public float fall_stop { get; set; } = 0f;

        public float amount { get; set; } = 0f;
        public float net_assets { get; set; } = 0f;
        public float amplitude { get; set; } = 0f;
        public float pb { get; set; } = 0f;
        public float benefit_before_tax { get; set; } = 0f;
        public float benefit_after_tax { get; set; } = 0f;
        public float convert_bond_ratio { get; set; } = 0f;
        public float totalissuescale { get; set; } = 0f;
        public float outstandingamt { get; set; } = 0f;

        public float circulation { get; set; } = 0f;
        public float par_value { get; set; } = 0f;
        public int after_hour_vol { get; set; } = 0;
        public float float_market_capital { get; set; } = 0f;
        public float volume_ratio { get; set; } = 0f;
        public float percent5m { get; set; } = 0f;
        public DateTime ex_dividend_date { get; set; } = DateTime.MinValue;
        public float moving_avg_200_day { get; set; } = 0f;
        public float chg_from_200_day_moving_avg { get; set; } = 0f;
        public float pct_chg_from_200_day_moving_avg { get; set; } = 0f;
        public float moving_avg_50_day { get; set; } = 0f;
        public float chg_from_50_day_moving_avg { get; set; } = 0f;
        public float pct_chg_from_50_day_moving_avg { get; set; } = 0f;
        public float ebitda { get; set; } = 0f;
        public float short_ratio { get; set; } = 0f;
        public float pe_estimate_next_year { get; set; } = 0f;
        public float peg_ratio { get; set; } = 0f;
        public float eps_estimate_next_year { get; set; } = 0f;
        public float eps_estimate_next_quarter { get; set; } = 0f;
        public float eps_estimate_current_quarter { get; set; } = 0f;
        public float psr { get; set; } = 0f;
        public float revenue { get; set; } = 0f;
        public float profit_margin { get; set; } = 0f;
    }
}
