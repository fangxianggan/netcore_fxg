using System;
using System.Collections.Generic;
using System.Text;
using UnitTest.DesignModel.DecoratorMode.Pay;

namespace UnitTest.DesignModel.DecoratorMode
{
  public  class TestClass
    {
       

        /// <summary>
        /// 流程
        /// </summary>
        public void TestData()
        {
            //1 支付
            //IPay pay = new PayClass();
            // pay.PayMoney(1000);

            //2 发送邮件
            //  AbstractBaseDoceator send = new SendEmail();

            //  send= new MessageClass();
            //  send = new BaseHandle(send,pay);


            //send.PayMoney(1000);

            //3 fasong xiaoxi
            //MessageClass message = new MessageClass();




            //  message.PayMoney(1000);

            AbstractBaseDoceator pay = new PayClass();
            pay = new SendEmail(pay);
            pay = new MessageClass(pay);
            pay = new BaseHandle(pay);
            pay.CallBackHandle(1000);




        }
    }
}
