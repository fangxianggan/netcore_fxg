using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.Core.Util
{
   public static class RandomUtil
    {


       public static string TimeStampNo()
       {
           Random random = new Random();
           string strRandom = random.Next(1000, 10000).ToString(); //生成编号 
           string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
           return code;
       }
    }
}
