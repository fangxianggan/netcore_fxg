using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ViewModel
{
   public class RabbitMQConfigViewModel
    {
        public string RabbitHost { set; get; }

        public string RabbitUserName { set; get; }

        public string RabbitPassword { set; get; }

        public int RabbitPort { set; get; }
    }
}
