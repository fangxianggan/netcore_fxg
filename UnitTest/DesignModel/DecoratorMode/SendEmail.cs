using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using UnitTest.DesignModel.DecoratorMode.Pay;

namespace UnitTest.DesignModel.DecoratorMode
{
    public class SendEmail : BaseHandle
    {
        public SendEmail(AbstractBaseDoceator doceator) : base(doceator)
        {
           
        }

        public override void CallBackHandle(decimal money)
        {
            base.CallBackHandle(money);
            SendEData();
        }

      
        private void SendEData()
        {
            Console.WriteLine("已经发送邮件");
        }
    }
}