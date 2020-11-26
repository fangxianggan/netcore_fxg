using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.DesignModel.DecoratorMode.Pay
{
    public class PayClass : AbstractBaseDoceator, IPay
    {
        public override void CallBackHandle(decimal money)
        {
            PayMoney(money);
        }

        public void PayMoney(decimal money)
        {
            Console.WriteLine("你已经支付了{0}", money);
        }
    }
}
