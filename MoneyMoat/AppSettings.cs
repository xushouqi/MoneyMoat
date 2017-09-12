using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyMoat
{
    public class AppSettings
    {
        public string GatewayHost { get; set; }
        public int GatewayPort { get; set; }

        public int TaskInterval { get; set; }
        public int TaskMaxCount { get; set; }


        public class RabbitMQConfig
        {
            public string EventBusConnection { get; set; }
            public string RabbitMQHostName { get; set; }
            public int RabbitMQPort { get; set; }
            public string RabbitMQUsername { get; set; }
            public string RabbitMQPassword { get; set; }
            public string RabbitMQVHost { get; set; }
        }
        public RabbitMQConfig RabbitMQConfigs { get; set; }

    }
}
