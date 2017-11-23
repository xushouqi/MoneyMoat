using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBConnector
{
    public class AppSettings
    {
        public string GatewayHost { get; set; }
        public int GatewayPort { get; set; }

        public int TaskInterval { get; set; }
        public int TaskMaxCount { get; set; }

    }
    public class ConnectionString
    {
        public string DefaultConnection { get; set; }
        public string MySQL { get; set; }
    }
}
