using System;
using System.Collections.Generic;
using System.Text;

namespace StockModels
{
    public class XueQiuWatch
    {
        public List<XueQiuStock> stocks { get; set; }
    }
    public class XueQiuStock
    {
        public string code { get; set; }
        public string exchange { get; set; }
    }
}
