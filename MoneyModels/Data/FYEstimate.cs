using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyModels
{
    public class FYEstimate : Entity
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

        public string type { get; set; }

        public string periodType { get; set; }

        public Int64 fYear { get; set; }
        public Int64 endMonth { get; set; }

        public float High { get; set; }
        public float Low { get; set; }
        public float Mean { get; set; }
        public float Median { get; set; }
        public float StdDev { get; set; }
        public int NumOfEst { get; set; }

    }
}
