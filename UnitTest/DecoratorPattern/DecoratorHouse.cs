using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.DecoratorPattern
{
    public class DecoratorHouse : IDecoratorHouse
    {
        public DecoratorHouse()
        {
            Console.WriteLine("我们开始装修房子！！！");
        }

        public void DecoratorData()
        {
            Console.WriteLine("开始凿墙。。。。");
        }
    }
}
