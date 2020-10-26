using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.DTO.ViewModel
{
   public class RedisConfigViewModel
    {
        public string WriteServerConStr { set; get; }

        public string ReadServerConStr { set; get; }
    }
}
