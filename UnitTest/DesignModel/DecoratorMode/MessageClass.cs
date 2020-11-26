using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DesignModel.DecoratorMode.Pay;

namespace UnitTest.DesignModel.DecoratorMode
{
    public class MessageClass : BaseHandle
    {
       
        public MessageClass(AbstractBaseDoceator doceator) :base(doceator)
        {
           
        }

        public override void CallBackHandle(decimal money)
        {
            base.CallBackHandle(money);
            SendMessData();
        }

       

        private void SendMessData()
        {
            Console.WriteLine("发送短信");
        }
    }
}
