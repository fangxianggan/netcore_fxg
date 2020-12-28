using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.ChainPattern
{
    public class TestRun
    {
        public TestRun()
        {


            RequestHandle request = new RequestHandle("小明", 10000);

            AbstractHandle handle1 = new Director("主管",new decimal[2]{ 0,10000});
            AbstractHandle handle2 = new Manager("经理",new decimal[2] { 10001,20000});
            AbstractHandle handle3 = new GManager("总经理",new decimal[2] { 20001,40000});
         
            handle1.NextHandle = handle2;
            handle2.NextHandle = handle3;

            handle1.HandleData(request);


        }
    }
}
